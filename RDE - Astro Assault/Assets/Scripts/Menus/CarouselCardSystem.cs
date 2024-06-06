using UnityEngine;
using UnityEngine.UI;

namespace RDE
{
    public class CarouselCardSystem : MonoBehaviour 
    {
        [Header("Carousel Settings")]
        [SerializeField] private RectTransform[] cardList;

        [Header("Helper Variables")]
        private ScrollRect scrollRect;
        private RectTransform viewportTransfrom;
        private RectTransform contentTransfrom;
        private float cardWidth;
        private bool horizontalLayout;
        private Vector2 oldVelocity;
        private bool isUpdated;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            viewportTransfrom = scrollRect.viewport;
            contentTransfrom = scrollRect.content;
            cardWidth = cardList[0].rect.width;

            if(contentTransfrom.GetComponent<HorizontalLayoutGroup>() != null)
            {
                cardWidth += contentTransfrom.GetComponent<HorizontalLayoutGroup>().spacing;
                horizontalLayout = true;
            }

            if (contentTransfrom.GetComponent<VerticalLayoutGroup>() != null)
            {
                cardWidth += contentTransfrom.GetComponent<VerticalLayoutGroup>().spacing;
                horizontalLayout = false;
            }
        }

        private void Start()
        {
            isUpdated = false;
            oldVelocity = Vector2.zero;

            MainMenuManager.instance.playerClass = "AegisSupport";

            for (int i = 0; i < cardList.Length; i++)
            {
                RectTransform rt = Instantiate(cardList[i % cardList.Length], contentTransfrom);
                rt.SetAsLastSibling();
            }

            for (int i = 0; i < cardList.Length; i++)
            {
                int num = cardList.Length - i - 1;
                while (num < 0)
                {
                    num += cardList.Length;
                }
                RectTransform rt = Instantiate(cardList[num], contentTransfrom);
                rt.SetAsFirstSibling();
            }

            contentTransfrom.localPosition = new Vector3(-cardWidth * cardList.Length, contentTransfrom.localPosition.y, contentTransfrom.localPosition.z);
        }

        private void Update()
        {
            if (isUpdated)
            {
                isUpdated = false;
                scrollRect.velocity = oldVelocity;
            }

            if (contentTransfrom.localPosition.x > 0f)
            {
                Canvas.ForceUpdateCanvases();
                oldVelocity = scrollRect.velocity;
                contentTransfrom.localPosition -= new Vector3(cardList.Length * cardWidth, 0f, 0f);
                isUpdated = true;
            }

            if (contentTransfrom.localPosition.x < -cardList.Length * cardWidth)
            {
                Canvas.ForceUpdateCanvases();
                oldVelocity = scrollRect.velocity;
                contentTransfrom.localPosition += new Vector3(cardList.Length * cardWidth, 0f, 0f);
                isUpdated = true;
            }
        }
    }
}