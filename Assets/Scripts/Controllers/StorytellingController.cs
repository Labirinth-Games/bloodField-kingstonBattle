using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HUD;
using UnityEngine;

namespace Controllers
{
    public class StorytellingController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private DialogHUD dialogHUD;

        [Header("Settings")]
        [SerializeField] private List<Story> book;
        [SerializeField] private int timeToDelayBetweenWords;
        [SerializeField] private int timeToDelayBetweenDialogs;

        private int _index = 0;

        public async Task Play(int index)
        {
            _index = index;

            var splitDialogs = book[_index].story.Split("\n");
            var i = 0;

            foreach (var dialog in splitDialogs)
            {
                dialogHUD.Clear();
                dialogHUD.SetText(dialog, timeToDelayBetweenWords);

                i++;

                if (i < splitDialogs.Count()-1)
                    await Task.Delay(timeToDelayBetweenDialogs);
            }

            book[_index].ShowElement?.SetActive(true);
            book[_index].action?.Invoke();
        }

        public async void Next()
        {
            if (_index + 1 < book.Count)
            {
                await Play(_index + 1);
            }
        }
        public async void Init()
        {
            await Play(0);
        }
    }

    [System.Serializable]
    public class Story
    {
        [TextArea]
        public string story;
        public GameObject ShowElement;
        public System.Action action;
    }
}