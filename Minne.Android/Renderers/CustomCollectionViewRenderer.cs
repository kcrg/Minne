using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Minne.Controls.CustomCollectionView), typeof(Minne.Droid.Renderers.CustomCollectionViewRenderer))]
namespace Minne.Droid.Renderers
{
    public class CustomCollectionViewRenderer : GroupableItemsViewRenderer<GroupableItemsView, GroupableItemsViewAdapterAdvanced<GroupableItemsView, IGroupableItemsViewSource>, IGroupableItemsViewSource>
    {
        public CustomCollectionViewRenderer(Context context) : base(context)
        {
        }
    }

    public class GroupableItemsViewAdapterAdvanced<TItemsView, TItemsViewSource> : GroupableItemsViewAdapter<TItemsView, TItemsViewSource>
            where TItemsView : GroupableItemsView
            where TItemsViewSource : IGroupableItemsViewSource
    {
        private readonly Dictionary<object, int> _viewHoldersViewTypesAssociations = new();
        private readonly CustomCollectionViewRenderer customCollectionViewRenderer;
        public GroupableItemsViewAdapterAdvanced(TItemsView groupableItemsView, CustomCollectionViewRenderer collectionViewAdvancedRenderer, Func<View, Context, ItemContentView>? createView = null)
            : base(groupableItemsView, createView)
        {
            customCollectionViewRenderer = collectionViewAdvancedRenderer;
        }

        public override int GetItemViewType(int position)
        {
            int itemViewType = base.GetItemViewType(position);

            //ensure all item types have its own placeholder and not a generic one TemplatedItem
            if (itemViewType == ItemViewType.TemplatedItem)
            {
                var itemType = ItemsSource.GetItem(position).GetType();

                if (!_viewHoldersViewTypesAssociations.ContainsKey(itemType))
                {
                    int viewTypeId;

                    if (_viewHoldersViewTypesAssociations.Count == 0)
                    {
                        //Framework TemplateItem View type is 42, ours are 421,422, ... 
                        viewTypeId = (ItemViewType.TemplatedItem * 10) + 1;
                    }
                    else
                    {
                        viewTypeId = _viewHoldersViewTypesAssociations.Max(assocation => assocation.Value) + 1;
                    }

                    //By default, Cells are not always recycled depending on state. Force to recycle the most mossible
                    //https://developer.android.com/reference/android/support/v7/widget/RecyclerView.RecycledViewPool.html#setMaxRecycledViews(int,%20int)
                    customCollectionViewRenderer.GetRecycledViewPool().SetMaxRecycledViews(viewTypeId, 200);

                    _viewHoldersViewTypesAssociations.Add(itemType, viewTypeId);
                }

                itemViewType = _viewHoldersViewTypesAssociations[itemType];
            }

            return itemViewType;
        }
    }
}