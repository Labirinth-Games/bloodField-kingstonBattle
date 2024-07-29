using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using BloodField.Managers;

namespace BloodField.Helpers
{
    class UIHelper : MonoBehaviour
    {
        public static async void AdditionalStatsUIRender(string text, GameObject target, int amountNotifications = 0)
        {
            var prefab = Resources.Load<GameObject>("UI/AdditionalStatusUIPrefab");
            var instance = Instantiate(prefab);
            instance.transform.SetParent(target.transform);

            instance.transform.position = target.transform.position;
            instance.GetComponent<TextMeshPro>().text = text;

            instance.transform.DOMoveY(target.transform.position.y + 1 + (amountNotifications * .6f), .5f).SetEase(Ease.InBounce);
            instance.transform.DOScale(0, .25f).From().SetEase(Ease.InBounce);

            await Task.Delay(1000);

            instance.transform.DOScale(0, .25f).SetEase(Ease.InBounce).OnComplete(() => Destroy(instance));

        }

        public static async void HitUIRender(string text, GameObject target)
        {
            var prefab = Resources.Load<GameObject>("UI/AdditionalStatusUIPrefab");
            var instance = Instantiate(prefab);
            // instance.transform.SetParent(target.transform);

            instance.transform.position = target.transform.position;
            var label = instance.GetComponent<TextMeshPro>();
            label.text = text;
            label.color = Color.red;

            instance.transform.DOMoveY(target.transform.position.y + 1 + .6f, .5f).SetEase(Ease.InBounce);
            instance.transform.DOScale(0, .25f).From().SetEase(Ease.InBounce);

            await Task.Delay(1000);

            instance.transform.DOScale(0, .25f).SetEase(Ease.InBounce).OnComplete(() => Destroy(instance));

        }
    }
}