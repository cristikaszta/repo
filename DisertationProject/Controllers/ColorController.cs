using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DisertationProject.Controllers
{
    public class ColorController
    {

        private List<View> _views;

        /// <summary>
        /// The color controller
        /// </summary>
        private static ColorController _colorController;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ColorController Instance
        {
            get
            {
                if (_colorController == null)
                    _colorController = new ColorController();
                return _colorController;
            }
        }

        private ColorController()
        {
            _views = new List<View>();
        }

        public void Add(View view)
        {
            view.Click += Highlight;
            _views.Add(view);

        }

        public void Add(IEnumerable<View> views)
        {
            foreach (var button in views)
            {
                Add(button);
            }
        }

        private void Highlight(object sender, EventArgs e)
        {
            var clickedView = (View)sender;
            clickedView.SetBackgroundColor(Color.Blue);
            var otherButtons = _views.Where(p => p.Id != clickedView.Id);
            foreach (var button in otherButtons)
            {
                button.SetBackgroundColor(Color.Gray);
            }
        }

        public void Remove(View view)
        {
            _views.Remove(view);
        }

    }
}