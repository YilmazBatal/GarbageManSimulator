using UnityEngine;

public class LidBehaviour : StateMachineBehaviour
{
    private AudioSource audioSource;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animasyon bittiğinde yapılacaklar
        // animator.GetComponent<MyScript>().DoSomething();
        ShakeCam();
    }

    private void ShakeCam()
    {
        audioSource = GameObject.Find("TrashTrigger").GetComponent<AudioSource>();
        GameObject.Find("Player").transform.GetChild(0).GetComponent<Unity.Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        audioSource.PlayOneShot(audioSource.clip);
    } 
}
