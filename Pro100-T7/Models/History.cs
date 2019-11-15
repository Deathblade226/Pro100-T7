using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pro100_T7.Model
{
	public class Action
	{
		public int layerID;
		//Layer info of some kind
	}
    public static class History
    {
		private static Stack<Action> undoStack = new Stack<Action>();
		private static Stack<Action> redoStack = new Stack<Action>();
		private static Action baseAction = new Action();



		/// <summary>
		/// Clears the history and redo stack to the initial state on load
		/// </summary>
		public static void ClearHistory()
		{
			historyStack.Clear();
			historyStack.Push(baseAction);
			redoStack.Clear();
			redoStack.Push(baseAction);
		}

		/// <summary>
		/// Initializes the class. Must be called whenever a new image is created or loaded and before any actions are taken
		/// </summary>
		/// <param name="currentState">Represents the current state of the image on loading.</param>
		public static void StartHistory(byte[] currentState)
		{
			Action temp = new Action(currentState);
			baseAction = temp;
			ClearHistory();
		}
		
		public static void EndAction(Action a)
		{
			if(redoStack.Count > 0)
			{
				redoStack.Clear();
			}
			undoStack.Push(a);
		}

		public static void Undo()
		{
			if(undoStack.Count > 0)
			{
				Action undoneAction = undoStack.Pop();
				redoStack.Push(undoneAction);
			}

		}

		public static void Redo()
		{
			if(redoStack.Count > 0)
			{
				Action redoneAction = redoStack.Pop();
				undoStack.Push(redoneAction);
			}
		}

    }
}
