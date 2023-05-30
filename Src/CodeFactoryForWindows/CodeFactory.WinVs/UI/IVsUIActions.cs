//*****************************************************************************
//* Code Factory SDK
//* Copyright (c) 2020-2023 CodeFactory, LLC
//*****************************************************************************

using System.Threading.Tasks;

namespace CodeFactory.WinVs.UI
{
    /// <summary>
    /// Definition of the user interface actions that are supported in visual studio.
    /// </summary>
    public interface IVsUIActions
    {
        /// <summary>
        /// Creates a new instance of a view that is supported in visual studio by code factory. This will load the <see cref="IVsActions"/> into the view as well as the logger that supports the view.
        /// </summary>
        /// <typeparam name="T">The type of visual studio user control to create.</typeparam>
        /// <returns>New instance of the target user control.</returns>
        /// <exception cref="VisualStudioException">Raises a visual studio error if there was a problem creating the user control. Review the internal exception for the source of the error.</exception>
        Task<T> CreateViewAsync<T>() where T : class,IView;

        /// <summary>
        /// Displays a dialog window in visual studio that hosts a view. This makes sure the dialog window is thread safe to be used with visual studio. 
        /// </summary>
        /// <param name="view">The view to be loaded into the dialog window.</param>
        /// <returns>Returns the result for the window which returns a true if a close event occurred, a false when a cancel event occurred, or null if neither were triggered.</returns>
        Task<bool?> ShowDialogWindowAsync(IView view);

        /// <summary>
        /// Displays a document panel with the target view imbedded in the document panel. 
        /// </summary>
        /// <param name="view">The view to be loaded into the document panel.</param>
        Task ShowDocumentPanelAsync(IView view);
    }
}
