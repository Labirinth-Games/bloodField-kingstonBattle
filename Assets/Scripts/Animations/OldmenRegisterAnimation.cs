using Controllers;
using UnityEngine;

namespace Animations
{
    public class OldmenRegisterAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private StorytellingController storytellingController; 

        public void StartTalk()
        {
            storytellingController.Init();
        }

        void Start()
        {
            animator?.SetTrigger("Walk");
        }
    }
}