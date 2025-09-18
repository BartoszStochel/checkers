using System.Collections;
using UnityEngine;

namespace Presentation
{
    public class PiecePresentation : MonoBehaviour
    {
        [field: SerializeField]
        public RectTransform RectTransform;
        [SerializeField]
        private float movementDurationInSeconds = 1;
        [SerializeField]
        private AnimationCurve movementCurve;

		private Vector2 movementBeginningPosition;
		private Vector2 movementTargetPosition;
		private float movementBeginningTime;

		public void MoveToPosition(Vector2 newPosition)
		{
			movementBeginningPosition = RectTransform.anchoredPosition;
			movementTargetPosition = newPosition;
			movementBeginningTime = Time.time;

			StopAllCoroutines();
			StartCoroutine(MovementCoroutine());
		}

		private IEnumerator MovementCoroutine()
		{
			while (RectTransform.anchoredPosition != movementTargetPosition)
			{
				var secondsSinceBeginningOfMovement = Time.time - movementBeginningTime;
				RectTransform.anchoredPosition = Vector2.Lerp(
					movementBeginningPosition,
					movementTargetPosition,
					movementCurve.Evaluate(secondsSinceBeginningOfMovement / movementDurationInSeconds));
				yield return null;
			}
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}
	}
}