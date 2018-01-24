﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DisertationProject.Model;

namespace DisertationProject.Adapters
{
    /// <summary>
    /// Custom song list adapter class
    /// Inherits BaseAdapter class
    /// </summary>
    public class SonglistAdapter : BaseAdapter<Song>
    {
        private List<Song> items;
        Activity context;

        public SonglistAdapter(Activity context)
        {
            this.context = context;
        }

        public SonglistAdapter(Activity context, List<Song> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override Song this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            if (convertView == null)
            {
                convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }
            convertView.FindViewById<TextView>(Android.Resource.Id.Text1).Text = string.Format("{0} - {1}", item.Name, item.Artist);

            return convertView;
        }
    }

    class SonglistAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}