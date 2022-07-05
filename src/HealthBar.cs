using flanne;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Healthbars
{
    internal class HealthBar : MonoBehaviour
    {
        private UnityAction<int> onHPChanged;
        internal RectTransform healthBar;
        private GameObject imgGameObject;
        private Health health;
        private Animation animation;
        private AnimationClip animationClip;

        void Awake()
        {
            AttachHealthbar();
            AttachAnimation();
        }

        private void AttachAnimation()
        {
            animation = imgGameObject.AddComponent<Animation>();
            animationClip = new AnimationClip();
            animationClip.legacy = true;
        }

        private void AttachHealthbar()
        {
            GameObject canvasGameObject = new GameObject("Healthbar");
            imgGameObject = new GameObject("HealthImage");
            canvasGameObject.AddComponent<Canvas>();

            healthBar = imgGameObject.AddComponent<RectTransform>();
            healthBar.transform.SetParent(canvasGameObject.transform);
            healthBar.localScale = Vector2.one;
            healthBar.anchoredPosition = new Vector2(0, 0.5f);
            healthBar.sizeDelta = new Vector2(1, 0.1f);

            Image image = imgGameObject.AddComponent<Image>();
            image.color = new Color(1f, 0f, 0f, 1f);
            imgGameObject.transform.SetParent(canvasGameObject.transform);
            canvasGameObject.transform.SetParent(gameObject.transform);

            health = GetComponentInParent<Health>();
            onHPChanged += UpdateHP;

            if (health is not null)
                health.onHealthChange.AddListener(onHPChanged);
        }

        private void UpdateHP(int hp)
        {
            if (health is not null)
            {
                AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, healthBar.sizeDelta.x, .5f, (float)hp / health.maxHP);
                animationClip.SetCurve("", typeof(RectTransform), "m_SizeDelta.x", animationCurve);

                if (animation.GetClipCount() > 0)
                {
                    animation.RemoveClip(animationClip);
                }

                animation.AddClip(animationClip, "HPAnim");
                animation.Play("HPAnim");
            }
        }
    }
}