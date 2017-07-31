using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace VisualSearchApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Your Cognitive Services Computer Vision API Key
            ApiKeys.computerVisionKey = "ea92a833d0f1467ab9c333c91bbd5942";

            // The code for where your endpoint is hosted: westus, eastus2, westcentralus, westeurope, souteheastasia
            ApiKeys.computerVisionHostSrv = "westcentralus"

            // Your Bing Web Search API Key
            ApiKeys.bingSearchKey = "c814652007004968837bbdb807408dbf";

            MainPage = new NavigationPage(new OcrSelectPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}