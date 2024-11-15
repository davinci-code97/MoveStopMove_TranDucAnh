using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.LightingExplorerTableColumn;

public class Player : Character
{
    public static Player Instance { get; private set; }

    public event EventHandler OnEnterObstacle;
    public event EventHandler OnExitObstacle;

    [SerializeField] private Joystick joystick;

    private Vector2 direction;

    private void Awake() {
        Instance = this;
    }

    protected override void Update()
    {
        base.Update();
        HandleMovement();

        if (charactersInRange.Count > 0) {
            FindNearestTarget();
        }

        if (rb.velocity.Equals(Vector3.zero) && nearestTarget != null) {
            Attack();
        }
    }

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (other.CompareTag(Constants.TAG_OBSTACLE)) {
            OnEnterObstacle?.Invoke(this, EventArgs.Empty);
        }
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        if (other.CompareTag(Constants.TAG_OBSTACLE)) {
            OnExitObstacle?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void OnKill(Character victim) {
        base.OnKill(victim);
        UIGamePlaying.Instance.UpdatePlayerScore(killCount);
    }

    // OnDeath
    protected override void Character_OnCharacterDead(object sender, OnCharacterDeadEventArgs e) {
        base.Character_OnCharacterDead(sender, e);
        joystick.gameObject.SetActive(false);
        joystick.gameObject.SetActive(true);
        GameManager.Instance.SetGameState(GameState.LOSE);
    }

    protected override void SetUpCurrentWeapon() {
        if (currentWeapon != null) {
            HBPool.Despawn(currentWeapon);
        }
        WeaponType weaponPoolType = UserDataManager.Instance.GetCurrentWeaponPoolType();
        WeaponConfig weaponConfig = LevelManager.Instance.GetWeaponConfigByType(weaponPoolType);
        SetUpWeapon(weaponConfig);
    }

    protected override void SetUpCurrentHat() {
        if (currentHat != null) {
            HBPool.Despawn(currentHat);
        }
        HatType hatType = UserDataManager.Instance.GetCurrentHatPoolType();
        if (hatType == HatType.None) return;
        HatConfig hatConfig = LevelManager.Instance.GetHatConfigByType(hatType);
        SetUpHat(hatConfig);
    }

    protected override void SetUpCurrentPants() {
        PantsType pantsType = UserDataManager.Instance.GetCurrentPantsType();
        if (pantsType == PantsType.None) {
            currentPants = pantsMeshRenderder.material = modelMeshRenderder.material;
            return;
        }
        PantsConfig pantsConfig = LevelManager.Instance.GetPantsConfigByType(pantsType);
        SetUpPants(pantsConfig);
    }

    protected override void FindNearestTarget() {
        base.FindNearestTarget();
        nearestDistance = Mathf.Infinity;
        if (charactersInRange.Count > 0) {
            foreach (Character character in charactersInRange) {
                Character target = character.GetComponent<Character>();
                float distance = Vector3.Distance(TF.position, target.TF.position);
                if (distance < nearestDistance) {
                    if (nearestTarget) {
                        nearestTarget.SetBeingTargetedSprite(false);
                    }
                    nearestDistance = distance;
                    nearestTarget = target;
                    nearestTarget.SetBeingTargetedSprite(true);
                }
                if (target != nearestTarget) {
                    target.SetBeingTargetedSprite(false);
                }
            }
        }
        else {
            nearestTarget = null;
        }
    }

    protected override void HandleMovement() {
        if (!joystick) return;
        base.HandleMovement();
        direction = joystick.Direction;
        rb.velocity = new Vector3(direction.x * moveSpeed, rb.velocity.y, direction.y * moveSpeed);
    }

    public Vector3 GetPlayerPosition() { return TF.position; }

    public void SetPlayerPosition(Vector3 newPosition) {
        TF.rotation = Quaternion.Euler(0, 180, 0);
        TF.position = newPosition;
    }

    public void OnWinGame() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.detectCollisions = false;
        nearestTarget = null;
        isAttacking = false;
        ChangeAnim(Constants.ANIM_DANCE);

        UserDataManager.Instance.SetPlayerGold(gold);
    }

}
