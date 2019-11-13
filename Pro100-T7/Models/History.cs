using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Pro100_T7.Models
{
	public class Action
	{
		public int layerID;
		public byte[] bmp;//Layer info of some kind

		public Action(byte[] bmp = null, int layerID = 0)
		{
			this.bmp = bmp;
			this.layerID = layerID;
		}
	}
    public static class History
    {
		private static Stack<Action> historyStack = new Stack<Action>();
		private static Stack<Action> redoStack = new Stack<Action>();

		//puts a clear default action at the start of the undo stack so we can undo the first action taken by the user. Call this to initialize the class
		/// <summary>
		/// Initializes the class. Must be called when the image is loaded before any actions are taken
		/// </summary>
		/// <param name="currentState">Represents the current state of the image on loading.</param>
		public static void StartHistory(byte[] currentState)
		{
			Action temp = new Action(currentState);
			historyStack.Push(temp);
			redoStack.Push(temp);
		}
		
		/// <summary>
		/// Takes in the state of the image to be recorded in the history stack.
		/// </summary>
		/// <param name="a">An action holding a byte array that represents the state of the image AFTER the action is taken.</param>
		public static void EndAction(Action a)
		{
			while(redoStack.Count > 1)
			{
				redoStack.Pop();
			}
			historyStack.Push(a);
		}

		//will return an ImageLayer or an action depends on how ImageLayer is implemented
		/// <summary>
		/// Undoes last taken action. 
		/// </summary>
		/// <returns>Returns an Action hold the byte array representing the state AFTER the undo is executed.</returns>
		public static Action Undo()
		{
			if(historyStack.Count > 1)
			{
				Action undoneAction = historyStack.Pop();
				redoStack.Push(undoneAction);
			}

			Action a = historyStack.Peek();
			return a;

		}

		/// <summary>
		/// Redoes last undone action. Clears after a new action is taken.
		/// </summary>
		/// <returns>Returns an Action holding the byte array representing the state AFTER the redo is executed.</returns>
		public static Action Redo()
		{
			if(redoStack.Count > 1)
			{
				Action redoneAction = redoStack.Pop();
				historyStack.Push(redoneAction);
			}

			Action a = historyStack.Peek();
			return a;
		}

    }
}