using UnityEngine;

public class IdleAttackPicker : StateMachineBehaviour
{
    [SerializeField] float minIdleTime = 1.0f;
    [SerializeField] float maxIdleTime = 2.5f;
    [SerializeField] bool avoidRepeatingAttack = true;

    static readonly int ChanceHash = Animator.StringToHash("Chance");

    float timer;
    bool picked;
    int lastPick = -1;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minIdleTime, maxIdleTime);
        picked = false;
        animator.SetInteger(ChanceHash, -1);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (picked) return;

        var attacks = animator.GetComponent<BennyAttacks>();
        if (attacks != null && attacks.IsBusy) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        int pick = Random.Range(0, 3); // 0 = Rain, 1 = Laser, 2 = Keyboard
        if (avoidRepeatingAttack && pick == lastPick)
            pick = (pick + Random.Range(1, 3)) % 3;

        lastPick = pick;
        picked = true;

        if (attacks != null) attacks.BeginAttack();

        animator.SetInteger(ChanceHash, pick);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(ChanceHash, -1);
    }
}