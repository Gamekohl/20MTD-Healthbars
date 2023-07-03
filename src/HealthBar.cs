using flanne;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static Healthbars.Plugin;
using UnityEngine.Animations;

namespace Healthbars
{
    internal class HealthBar : MonoBehaviour
    {
        private UnityAction<int> onHPChanged;
        internal RectTransform healthBar;
        private GameObject canvasGameObject;
        private GameObject imgGameObject;
        private Health health;
        private Animation animation;
        private AnimationClip animationClip;
        private Quaternion initRotation;

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

            health = GetComponentInParent<Health>();
            onHPChanged += UpdateHP;

            if (health is not null)
            {
                health.onHealthChange.AddListener(onHPChanged);

                if (animation.GetClipCount() == 0)
                    animation.AddClip(animationClip, "HPAnim");
            }
        }

        private void AttachHealthbar()
        {
            canvasGameObject = new GameObject("Healthbar");
            imgGameObject = new GameObject("HealthImage");
            canvasGameObject.AddComponent<Canvas>();

            healthBar = imgGameObject.AddComponent<RectTransform>();
            healthBar.transform.SetParent(canvasGameObject.transform);
            healthBar.localScale = Vector2.one;
            healthBar.anchoredPosition = new Vector2(0, gameObject.GetComponentInParent<SpriteRenderer>().size.y / 2);
            healthBar.sizeDelta = new Vector2(1, 0.1f);

            Image image = imgGameObject.AddComponent<Image>();
            image.color = new Color(1f, 0f, 0f, 1f);
            imgGameObject.transform.SetParent(canvasGameObject.transform);
            canvasGameObject.transform.SetParent(gameObject.transform);

            initRotation = canvasGameObject.transform.rotation;
        }

        private void UpdateHP(int hp)
        {
            animation.GetClip("HPAnim").ClearCurves();
            AnimationCurve animationCurve = AnimationCurve.EaseInOut(0f, healthBar.sizeDelta.x, .5f, (float)hp / health.maxHP);
            animation.GetClip("HPAnim").SetCurve("", typeof(RectTransform), "m_SizeDelta.x", animationCurve);
            animation.Play("HPAnim");
        }

        //lock canvas rotation to prevent healthbars from orbiting spinny enemies
        void LateUpdate()
        {
            canvasGameObject.transform.rotation = initRotation;
        }
    }
}