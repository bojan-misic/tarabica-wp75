using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Tarabica.WP7App.ViewModels;
using System.Text;

namespace Tarabica.WP7App.Views
{
    public partial class SessionDetailsView : PhoneApplicationPage
    {
        public SessionDetailsView()
        {
            InitializeComponent();
            //var vm = DataContext as ViewModel;
            //vm.PropertyChanged += vm_PropertyChanged;
            //WebBrowser.LoadCompleted += (s, e) =>
            //{
            //    string height = (string)WebBrowser.InvokeScript("eval", "getDocHeight()");
            //    if (!string.IsNullOrEmpty(height))
            //    {
            //        WebBrowser.Height = int.Parse(height);
            //        OverlayRectangle.Height = int.Parse(height);
            //    }

            //};
        }

        //void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName != "Description")
        //        return;

        //    //var f = "function getDocHeight() {" +
        //    //                "var D = document;" +
        //    //                "return Math.max(" +
        //    //                    "Math.max(D.body.scrollHeight, D.documentElement.scrollHeight)," +
        //    //                    "Math.max(D.body.offsetHeight, D.documentElement.offsetHeight)," +
        //    //                    "Math.max(D.body.clientHeight, D.documentElement.clientHeight) " +
        //    //                ").toString();" +
        //    //            "}";

        //    var f = "function getDocHeight() {" +
        //        "var D = document;" +
        //        "return D.documentElement.scrollHeight" +
        //        ".toString();" +
        //    "}";

        //    var description = (DataContext as SessionDetailsViewModel).Description;
        //    if (!string.IsNullOrEmpty(description))
        //    {
        //        WebBrowser.NavigateToString("<html><head><script type='text/javascript'>" + f + "</script><style>*{color: 'white'; background: 'black'; font-family:'Segoe UI';}</style></head><body>" + description + "</body></html>");
        //    }
        //}
    }
}