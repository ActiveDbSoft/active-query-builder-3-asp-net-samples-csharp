using System;
using System.Web.UI;
using ActiveQueryBuilder.Web.Server;

namespace WebForms_Samples.Samples
{
    public class BasePage : Page
    {
        //CUT:PRO{{
        public BasePage()
        {
            PreLoad += OnPreLoad;
        }

        private void OnPreLoad(object sender, EventArgs eventArgs)
        {
            QueryBuilderStore.Remove();
            QueryTransformerStore.Remove();
        }
        //}}CUT:PRO
    }
}