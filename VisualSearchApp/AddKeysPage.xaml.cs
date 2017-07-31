using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualSearchApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddKeysPage : ContentPage
	{
        #region constants
        // The media type of the body sent to the API. "application/octet-stream" defines an image represented as a byte array
        const string PhotoContentType = "application/octet-stream";
        const string SubscriptionKeyHeader = "Ocp-Apim-Subscription-Key";
        // URIs of the endpoints used in the test requests
        const string searchUri = "https://api.cognitive.microsoft.com/bing/v5.0/search?q=test";
        #endregion

        #region fields
        // booleans set when the keys are proven to work
        private bool computerVisionKeyWorks = false;
        private bool bingSearchKeyWorks = false;
        private string endpointLoc = String.Empty;
        HttpClient VisionApiClient;
        #endregion

        
        public AddKeysPage ()
		{
			InitializeComponent();
		}

        // send a test POST request to see if the Vision API Key is functional
        async Task CheckComputerVisionKey(object sender = null, EventArgs e = null)
        {
            // Empty image for test OCR request
            byte[] emptyImage = new byte[10];
            HttpResponseMessage response;
            VisionApiClient = new HttpClient();

            VisionApiClient.DefaultRequestHeaders.Add(SubscriptionKeyHeader, ComputerVisionKeyEntry.Text);
            using (var content = new ByteArrayContent(emptyImage))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(PhotoContentType);
                response = await VisionApiClient.PostAsync($"https://{endpointLoc}.api.cognitive.microsoft.com/vision/v1.0/ocr?", content);
            }
            if ((int)response.StatusCode != 401)
            {
                ComputerVisionKeyEntry.BackgroundColor = Color.Green;
                ApiKeys.computerVisionKey = ComputerVisionKeyEntry.Text;
                computerVisionKeyWorks = true;
            }
            else
            {
                ComputerVisionKeyEntry.BackgroundColor = Color.Red;
                computerVisionKeyWorks = false;
            }
        }

        // send a test GET request to see if the Bing Search API key is functional
        async Task CheckBingSearchKey(object sender = null, EventArgs e = null)
        {
            HttpResponseMessage response;
            HttpClient SearchApiClient = new HttpClient();

            SearchApiClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingSearchKeyEntry.Text);

            response = await SearchApiClient.GetAsync(searchUri);
            if ((int)response.StatusCode != 401)
            {
                BingSearchKeyEntry.BackgroundColor = Color.Green;
                ApiKeys.bingSearchKey = BingSearchKeyEntry.Text;
                bingSearchKeyWorks = true;
            }
            else
            {
                BingSearchKeyEntry.BackgroundColor = Color.Red;
                bingSearchKeyWorks = false;
            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                endpointLoc = picker.Items[selectedIndex];
            }
        }

        async void TryToAddKeys(object sender, EventArgs e)
        {
            if (!computerVisionKeyWorks)
                await CheckComputerVisionKey();
            if (!bingSearchKeyWorks)
                await CheckBingSearchKey();

            if (bingSearchKeyWorks && computerVisionKeyWorks)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error","One or more of your keys are invalid.  Please update them and try again", "OK");
            }
        }
    }
}