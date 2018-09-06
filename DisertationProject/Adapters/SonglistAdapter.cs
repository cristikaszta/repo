using Android.App;
using Android.Views;
using Android.Widget;
using DisertationProject.Model;
using System.Collections.Generic;

namespace DisertationProject.Adapters
{
    /// <summary>
    /// Custom song list adapter class
    /// Inherits BaseAdapter class
    /// </summary>
    /// <seealso cref="Android.Widget.BaseAdapter{DisertationProject.Model.Song}" />
    public class SonglistAdapter : BaseAdapter<Song>
    {
        /// <summary>
        /// The items
        /// </summary>
        private List<Song> items;

        /// <summary>
        /// The context
        /// </summary>
        private Activity context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SonglistAdapter" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SonglistAdapter(Activity context)
        {
            this.context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SonglistAdapter" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="items">The items.</param>
        public SonglistAdapter(Activity context, List<Song> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        /// <summary>
        /// Gets the <see cref="Song" /> with the specified position.
        /// </summary>
        /// <value>
        /// The <see cref="Song" />.
        /// </value>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public override Song this[int position]
        {
            get
            {
                return items[position];
            }
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <value>
        /// To be added.
        /// </value>
        /// <remarks>
        /// To be added.
        /// </remarks>
        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }


        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="position">To be added.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// To be added.
        /// </remarks>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// To be added.
        /// </summary>
        /// <param name="position">To be added.</param>
        /// <param name="convertView">To be added.</param>
        /// <param name="parent">To be added.</param>
        /// <returns>
        /// To be added.
        /// </returns>
        /// <remarks>
        /// To be added.
        /// </remarks>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = string.Format("{0} - {1}", item.Title, item.Artist);

            return convertView;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Java.Lang.Object" />
    internal class SonglistAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}