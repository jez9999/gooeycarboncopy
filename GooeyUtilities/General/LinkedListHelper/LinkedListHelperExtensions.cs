using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GooeyUtilities.General.LinkedListHelper {
	public static class LinkedListHelperExtensions {
		/// <summary>
		/// Searches linked list nodes using the given predicate starting from the specified node, going backwards toward the first node in the linked list.
		/// </summary>
		/// <typeparam name="T">The type of object the linked list is comprised of.</typeparam>
		/// <param name="thisNode">The node to start the search from.</param>
		/// <param name="searchThisNode">If true, searches the specified node first; otherwise, searches the node previous to the specified node first.</param>
		/// <param name="predicate">A delegate to execute which should evaluate the passed-in object and return true if it matches the search parameters, or false otherwise.</param>
		/// <returns>If any matching nodes exist previous to the specified node, the first found matching node; otherwise null.</returns>
		public static LinkedListNode<T> FindFirstBefore<T>(this LinkedListNode<T> thisNode, bool searchThisNode, Func<T, bool> predicate) {
			LinkedListNode<T> currentNode = (searchThisNode ? thisNode : thisNode.Previous);

			while (currentNode != null) {
				if (predicate(currentNode.Value)) {
					break;
				}
				currentNode = currentNode.Previous;
			}

			return currentNode;
		}

		/// <summary>
		/// Searches linked list nodes using the given predicate starting from the specified node, going forwards toward the last node in the linked list.
		/// </summary>
		/// <typeparam name="T">The type of object the linked list is comprised of.</typeparam>
		/// <param name="thisNode">The node to start the search from.</param>
		/// <param name="searchThisNode">If true, searches the specified node first; otherwise, searches the node subsequent to the specified node first.</param>
		/// <param name="predicate">A delegate to execute which should evaluate the passed-in object and return true if it matches the search parameters, or false otherwise.</param>
		/// <returns>If any matching nodes exist subsequent to the specified node, the first found matching node; otherwise null.</returns>
		public static LinkedListNode<T> FindFirstAfter<T>(this LinkedListNode<T> thisNode, bool searchThisNode, Func<T, bool> predicate) {
			LinkedListNode<T> currentNode = (searchThisNode ? thisNode : thisNode.Next);

			while (currentNode != null) {
				if (predicate(currentNode.Value)) {
					break;
				}
				currentNode = currentNode.Next;
			}

			return currentNode;
		}

		/// <summary>
		/// Searches a linked list using the given predicate, going forwards from the first node toward the last node in the linked list.
		/// </summary>
		/// <typeparam name="T">The type of object the linked list is comprised of.</typeparam>
		/// <param name="thisList">The linked list to search.</param>
		/// <param name="predicate">A delegate to execute which should evaluate the passed-in object and return true if it matches the search parameters, or false otherwise.</param>
		/// <returns>If any matching nodes exist in the linked list, the first found matching node (the one nearest the start of the list); otherwise null.</returns>
		public static LinkedListNode<T> FindFirstForwards<T>(this LinkedList<T> thisList, Func<T, bool> predicate) {
			if (thisList.First == null) {
				return null;
			}
			return thisList.First.FindFirstAfter(true, predicate);
		}

		/// <summary>
		/// Searches a linked list using the given predicate, going backwards from the last node toward the first node in the linked list.
		/// </summary>
		/// <typeparam name="T">The type of object the linked list is comprised of.</typeparam>
		/// <param name="thisList">The linked list to search.</param>
		/// <param name="predicate">A delegate to execute which should evaluate the passed-in object and return true if it matches the search parameters, or false otherwise.</param>
		/// <returns>If any matching nodes exist in the linked list, the last found matching node (the one nearest the end of the list); otherwise null.</returns>
		public static LinkedListNode<T> FindLastBackwards<T>(this LinkedList<T> thisList, Func<T, bool> predicate) {
			if (thisList.Last == null) {
				return null;
			}
			return thisList.Last.FindFirstBefore(true, predicate);
		}
	}
}
