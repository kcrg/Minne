using Android.Content;
using Android.Util;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Core.View;
using Google.Android.Material.AppBar;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Snackbar;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using LP = Android.Views.ViewGroup.LayoutParams;

[assembly: ExportRenderer(typeof(Minne.Views.AppShell), typeof(Minne.Droid.Renderers.CustomShellRenderer))]
namespace Minne.Droid.Renderers
{
    public class CustomShellRenderer : ShellRenderer
    {
        public CustomShellRenderer(Context context) : base(context) { }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            base.CreateToolbarAppearanceTracker();
            return new CustomShellToolbarAppearanceTracker(this);
        }

        //protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        //{
        //    base.CreateBottomNavViewAppearanceTracker(shellItem);
        //    return new CustomShellBottomNavViewAppearanceTracker();
        //}
    }

    public class CustomShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
    {
        public CustomShellToolbarAppearanceTracker(IShellContext shellContext) : base(shellContext)
        {
        }

        public override void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);

            //toolbar.LayoutParameters = new AppBarLayout.LayoutParams(LP.MatchParent, LP.WrapContent)
            //{
            //    ScrollFlags = AppBarLayout.LayoutParams.ScrollFlagScroll
            //    //| AppBarLayout.LayoutParams.ScrollFlagEnterAlways,
            //};
        }
    }

    //public class CustomShellBottomNavViewAppearanceTracker : IShellBottomNavViewAppearanceTracker
    //{
    //    public void Dispose()
    //    {
    //    }

    //    public void ResetAppearance(BottomNavigationView bottomView)
    //    {
    //    }

    //    public void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
    //    {
    //        bottomView.Elevation = 8f;

    //        //bottomView.beha

    //        //bottomView.LayoutParameters = new BottomNavigationView.LayoutParams(LP.MatchParent, LP.WrapContent)
    //        //{
    //        //    Lay
    //        //};
    //    }
    //}

    //public class CustomBehavior : CoordinatorLayout.Behavior
    //{
    //    public CustomBehavior() : base()
    //    {
    //    }

    //    public CustomBehavior(Context context, IAttributeSet attrs) : base(context, attrs)
    //    {
    //    }

    //    public override bool LayoutDependsOn(CoordinatorLayout parent, Object child, Android.Views.View dependency)
    //    {
    //        base.LayoutDependsOn(parent, child, dependency);

    //        bool dependsOn = dependency is FrameLayout;
    //        return dependsOn;
    //    }

    //    public override bool OnStartNestedScroll(CoordinatorLayout coordinatorLayout, Object child, Android.Views.View directTargetChild, Android.Views.View target, int axes, int type)
    //    {
    //        return axes == ViewCompat.ScrollAxisVertical;
    //    }

    //    public override void OnNestedPreScroll(CoordinatorLayout coordinatorLayout, Object child, Android.Views.View target, int dx, int dy, int[] consumed, int type)
    //    {
    //        base.OnNestedPreScroll(coordinatorLayout, child, target, dx, dy, consumed, type);

    //        if (child is null)
    //        {
    //            return;
    //        }

    //        if (dy < 0)
    //        {
    //            ShowBottomNavigationView(child);
    //        }
    //        else if (dy > 0)
    //        {
    //            HideBottomNavigationView(child);
    //        }
    //    }

    //    private void HideBottomNavigationView(Object child)
    //    {
    //        if (child is null)
    //        {
    //            return;
    //        }

    //        var view = child as BottomNavigationView;

    //        view?.Animate()?.TranslationY(view.Height);
    //    }

    //    private void ShowBottomNavigationView(Object child)
    //    {
    //        if (child is null)
    //        {
    //            return;
    //        }

    //        var view = child as BottomNavigationView;

    //        view?.Animate()?.TranslationY(0);
    //    }

    //    private void UpdateSnackbar(Object child, Android.Views.View snackbarLayout)
    //    {
    //        snackbarLayout.LayoutParameters = new Snackbar.SnackbarLayout.LayoutParams(LP.MatchParent, LP.WrapContent)
    //        {
    //            Gravity = Android.Views.GravityFlags.Top,
    //        };
    //    }
    //}
}
