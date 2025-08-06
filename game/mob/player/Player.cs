using Godot;
using teos.common.command;
using teos.common.path;
using teos.game.data;
using teos.game.stage.character_manager;
using teos.game.stage.hud;
using teos.game.system;
using teos.game.weapon;

namespace teos.game.mob.player;

/// <summary>
/// 主人公
/// </summary>
public partial class Player : Fighter, ICharacterManager
{
    [Export]
    public Node2D Target { get; set; }

    [Export]
    public float DefaultAutoScrollSpeed { get; set; } = 90f;

    [Export]
    public ReferenceRect BorderRect { get; set; }

    [Export]
    public Hud Hud { get; set; }

    private PlayerData _playerData;
    private MountPoint _mountPoint;
    private float _defaultAcceleration;
    private float _defaultReductionAcceleration;
    private AnimationTree _animationTree;
    private CharacterManager _characterManager;
    private Vector2 _velocity;
    private bool _gameEnded = false;
    private PathFollow _autoScrollPathFollow;

    protected AnimationNodeStateMachinePlayback m_StateMachine;

    public override void _Ready()
    {
        _mountPoint = GetNode<MountPoint>("MountPoint");
        _animationTree = GetNode<AnimationTree>("AnimationTree");
        _autoScrollPathFollow = GetParentOrNull<PathFollow>();
        m_StateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        base._Ready();
        AddToGroup(CharacterManager.CharacterGroup);
    }

    public override void _Process(double delta)
    {
        Vector2 input = Input.GetVector("game_left", "game_right", "game_up", "game_down");

        switch (m_StateMachine.GetCurrentNode())
        {
            case "stage_victory":
                break;

            case "separate_weapon" or "resurrected":

                CalcVelocity(delta, input);
                CalcPosition(delta);
                break;

            case "damaged":

                CalcVelocity(delta, input);
                CalcPosition(delta);
                WeaponRotate(delta);
                break;

            case "idle":

                CalcVelocity(delta, input);
                CalcPosition(delta);
                WeaponRotate(delta);

                if (_mountPoint.GetWeapon() is WeaponRoot weapon)
                {
                    if (Input.IsActionPressed("game_a") && Input.IsActionPressed("game_b"))
                    {
                        m_StateMachine.Travel("separate_weapon");
                    }
                    else
                    {
                        bool fire = Input.IsActionPressed("game_a") || Input.IsActionPressed("game_b");
                        weapon.Update(fire);
                    }

                    Hud?.UpdateWeapon(weapon);
                }

                break;
        }
    }

    private void CalcPosition(double delta)
    {
        Vector2 position = Position;
        position += _velocity * (float)delta;

        if (BorderRect is not null)
        {
            Vector2 begin = BorderRect.GetBegin();
            Vector2 end = BorderRect.GetEnd();
            Vector2 new_position = position.Clamp(begin, end);
            Position = new_position;

            // 端に到達した場合は速度がゼロ
            if (new_position != position)
            {
                Vector2 velocity = _velocity;

                if (new_position.X != position.X)
                {
                    velocity.X = 0f;
                }

                if (new_position.Y != position.Y)
                {
                    velocity.Y = 0f;
                }

                _velocity = velocity;
            }
        }
        else
        {
            Position = position;
        }
    }

    private void WeaponRotate(double delta)
    {
        // 武器の回転
        Vector2 targetPosition = Target.GlobalPosition;

        if (targetPosition.Length() != 0)
        {
            _mountPoint.RotateWeapon(delta, GlobalPosition.AngleToPoint(targetPosition));
        }
    }

    private void CalcVelocity(double delta, Vector2 input)
    {
        // 移動
        Vector2 velocity = _velocity;
        int signX = Mathf.Sign(input.X);

        if (signX == 0)
        {
            //a
            velocity.X = Mathf.MoveToward(velocity.X, 0f, m_ReductionAcceleration * (float)delta);
        }
        else if (signX == -Mathf.Sign(velocity.X))
        {
            //b
            velocity.X = Mathf.MoveToward(velocity.X, 0f, (m_ReductionAcceleration + m_Acceleration) * (float)delta);
        }
        else
        {
            //c
            velocity.X = Mathf.MoveToward(velocity.X, signX * m_MaxSpeed, m_Acceleration * (float)delta);
        }

        int signY = Mathf.Sign(input.Y);

        if (signY == 0)
        {
            //a
            velocity.Y = Mathf.MoveToward(velocity.Y, 0f, m_ReductionAcceleration * (float)delta);
        }
        else if (signY == -Mathf.Sign(velocity.Y))
        {
            //b
            velocity.Y = Mathf.MoveToward(velocity.Y, 0f, (m_ReductionAcceleration + m_Acceleration) * (float)delta);
        }
        else
        {
            //c
            velocity.Y = Mathf.MoveToward(velocity.Y, signY * m_MaxSpeed, m_Acceleration * (float)delta);
        }

        _velocity = velocity;
    }

    public void GameOver()
    {
        SeparateWeaponOnly();
        CommandRoot.ExecChildren(GetNodeOrNull("GameOver"), this, true);
    }

    public void StageVictory()
    {
        SeparateWeaponOnly();
        CommandRoot.ExecChildren(GetNodeOrNull("StageVictory"), this, true);
    }

    public void Resurrected()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Resurrected"), this, true);
    }

    public void AddRemain(int value)
    {
        _playerData.AddRemain(value);
        Hud?.UpdateRemain(_playerData.Remain);
        PlaySeOneup(value);
    }

    private async void PlaySeOneup(int value)
    {
        for (int i = 0; i < value; i++)
        {
            PlaySe("item_oneup");
            _ = await ToSignal(GetTree().CreateTimer(0.2f, false), Timer.SignalName.Timeout);
        }
    }

    public override void EquipWeapon(WeaponRoot weapon, bool instantly)
    {
        if ((m_StateMachine.GetCurrentNode() == "idle" || instantly) && _mountPoint.EquipWeapon(this, weapon, false, instantly))
        {
            _velocity = Vector2.Zero;
            Hud?.UpdateWeapon(weapon);
            base.EquipWeapon(weapon, instantly);
        }
    }

    public override void SeparateWeapon()
    {
        if (_mountPoint.GetWeapon() is not null)
        {
            SeparateWeaponOnly();
            base.SeparateWeapon();
        }
    }

    private void SeparateWeaponOnly()
    {
        _mountPoint.SeparateWeapon(this);
        Hud?.UpdateWeapon(null);
    }

    public void ResetLife()
    {
        Life = InitialLife;
        Hud?.UpdateLife(Life);
    }

    public override void Burialed(Node2D node)
    {
        AddLife(-1);
    }

    public void DamageControl()
    {
        m_StateMachine.Travel(Life == 0 ? "dead" : "damaged");
    }

    public void JudgeGameOver()
    {
        if (_playerData.Remain == 0)
        {
            m_StateMachine.Start("game_over");
        }
    }

    public void SetStageVictory(bool gameEnded)
    {
        _gameEnded = gameEnded;
        m_StateMachine.Start("stage_victory");
    }

    public void AddScore(int score)
    {
        PlaySeOneup(_playerData.AddScore(score));
        Hud?.UpdateScore(_playerData.Score);
        Hud?.UpdateRemain(_playerData.Remain);
    }

    public override void UpdateAutoScrollSpeed(float? speed)
    {
        float autoScrollSpeed = DefaultAutoScrollSpeed;

        if (speed is not null)
        {
            autoScrollSpeed = (float)speed;
        }

        _autoScrollPathFollow?.SetSpeed(autoScrollSpeed);
    }

    public static Player GetPlayer(Node root)
    {
        return root.GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().GetNode<Player>("%Player");
    }

    #region IGameNodeインタフェース
    public override void InitializeNode()
    {
        base.InitializeNode();
        _playerData = GetNode<GameDataManager>("/root/GameDataManager").GetPlayerData();
    }
    #endregion

    #region ILifeインタフェース
    public override void AddLife(int value)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            // プレーヤーへのダメージはレーザーとソード以外は-1固定
            if (value is < 0 and >= (-5))
            {
                value = -1;
            }

            base.AddLife(value);
            Hud?.UpdateLife(Life);

            if (value < 0)
            {
                m_StateMachine.Travel("damage_control");
            }
        }
    }

    public override void Dead()
    {
        _velocity = Vector2.Zero;
        SeparateWeaponOnly();
        base.Dead();
    }
    #endregion

    #region ICharacterManagerインタフェース
    public void SetCharacterManager(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public void ActiveCharacter(bool active)
    {
        m_StateMachine.Start(active ? "initialize" : "sleep");
    }

    public void InitializeCharacter()
    {
        Hud?.UpdateScore(_playerData.Score);
        Hud?.UpdateLife(Life);
        Hud?.UpdateWeapon(_mountPoint.GetWeapon());
        Hud?.UpdateRemain(_playerData.Remain);
        UpdateAutoScrollSpeed(DefaultAutoScrollSpeed);
        CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);
    }

    public void TerminateCharacter()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
    }
    #endregion
}
