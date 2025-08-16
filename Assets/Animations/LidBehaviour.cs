using UnityEngine;

public class LidBehaviour : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animasyon bittiğinde yapılacaklar
        // animator.GetComponent<MyScript>().DoSomething();
        ShakeCam();
    }

    private void ShakeCam()
    {
        GameObject.Find("Player").transform.GetChild(0).GetComponent<Unity.Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
    } 
}
