using UnityEngine;
using UnityEngine.UI;
using System;

namespace Presentation
{
    public class TilePresentation : MonoBehaviour
    {
        [field: SerializeField]
        public RectTransform RectTransform { get; private set; }

        [SerializeField]
        public Button button;
        [SerializeField]
        public GameObject moveHighlight;
        [SerializeField]
        public GameObject attackHighlight;
        [SerializeField]
        public GameObject attackTargetHighlight;

        public void SetOnButtonClickedAction(Action action)
		{
            button.onClick.AddListener(action.Invoke);
		}

        public void ActivateMoveHighlight()
		{
            moveHighlight.SetActive(true);
            attackHighlight.SetActive(false);
            attackTargetHighlight.SetActive(false);
        }

        public void ActivateAttackHighlight()
		{
            moveHighlight.SetActive(false);
            attackHighlight.SetActive(true);
            attackTargetHighlight.SetActive(false);
        }

        public void ActivateAttackTargetHighlight()
		{
            moveHighlight.SetActive(false);
            attackHighlight.SetActive(false);
            attackTargetHighlight.SetActive(true);
        }

        public void DeactivateHighlights()
		{
            moveHighlight.SetActive(false);
            attackHighlight.SetActive(false);
            attackTargetHighlight.SetActive(false);
        }
    }
}