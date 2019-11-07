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

		//puts a clear default action at the start of the undo stack so we can undo the first action taken by the user. Call this to initialize the class
		public static void StartHistory()
		{
			undoStack.Push(new Action());
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
