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
		private static Stack<Action> undoStack = new Stack<Action>();
		private static Stack<Action> redoStack = new Stack<Action>();

		//puts a clear default action at the start of the undo stack so we can undo the first action taken by the user. Call this to initialize the class
		public static void StartHistory(byte[] currentState)
		{
			Action temp = new Action(currentState);
			undoStack.Push(temp);
			redoStack.Push(temp);
		}
		
		public static void EndAction(Action a)
		{
			while(redoStack.Count > 1)
			{
				redoStack.Pop();
			}
			undoStack.Push(a);
		}

		//will return an ImageLayer or an action depends on how ImageLayer is implemented
		//returns the image how it should look AFTER the undo is executed
		public static Action Undo()
		{
			if(undoStack.Count > 1)
			{
				Action undoneAction = undoStack.Pop();
				redoStack.Push(undoneAction);
			}

			Action a = undoStack.Peek();
			return a;

		}

		//Not finished 
		public static Action Redo()
		{
			if(redoStack.Count > 1)
			{
				Action redoneAction = redoStack.Pop();
				undoStack.Push(redoneAction);
			}

			Action a = redoStack.Peek();
			return a;
		}

    }
}
