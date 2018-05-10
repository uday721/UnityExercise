using UnityEngine;
using System.Collections;


/// <summary>
/// Destroys this GameObject when the Animator attached, has finished playing animations.
/// </summary>
[RequireComponent(typeof(Animator))]
public class DestroyAfterAnim : MonoBehaviour
{
	Animator anim;


	void Awake()
	{
		anim = this.GetComponent<Animator>();
	}


	void Update()
	{
		if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
		{
			// Destroy this object after the animation has finished.
			Destroy(this.gameObject);
		}
	}
}
