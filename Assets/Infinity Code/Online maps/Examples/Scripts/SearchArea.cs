using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InfinityCode.OnlineMapsDemos
{
    public class SearchArea:MonoBehaviour
    {
        public string googleAPIKey;
        public InputField inputField;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(googleAPIKey)) Debug.LogWarning("Please specify Canvas / Search Area / Search Area (Script) / Google API Key");
        }

        public void Search()
        {
            string locationName = inputField.text;
            if (string.IsNullOrEmpty(locationName) || locationName.Length < 3) return;

            OnlineMapsGoogleGeocoding request = new OnlineMapsGoogleGeocoding(locationName, googleAPIKey);
            request.OnComplete += OnGeocodingComplete;
            request.Send();
        }

        private void OnGeocodingComplete(string response)
        {
            if (string.IsNullOrEmpty(response)) return;
            Vector2 position = OnlineMapsGoogleGeocoding.GetCoordinatesFromResult(response);
            if (position != Vector2.zero) OnlineMaps.instance.position = position;
        }

        private void Update()
        {
            EventSystem eventSystem = EventSystem.current;
            if ((Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return)) && eventSystem.currentSelectedGameObject == inputField.gameObject)
            {
                Search();
            }
        }
    }
}
