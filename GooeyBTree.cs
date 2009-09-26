using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gooey
{
	/// <summary>
	/// The main point of this structure is that stored values are inherently ordered.  When a value is inserted
	/// (the class can be accessed using an indexer), it is automatically ordered in the tree structure using the
	/// IComparable implemented by the TKey type.  Ergo, the tree can later be walked from 'smallest' to 'largest'
	/// or vice versa, according to the ordering of the nodes.
	/// 
	/// Microsoft's SortedList and SortedDictionary basically implement this, save for the ability to throw an
	/// exception if the key already exists.
	/// </summary>
	public class GooeyBTree<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey> {
		// Gooey Software binary tree class, v1.2
		// TODO: Allow a custom IComparable to be specified for key comparison?
		private GooeyBTreeNode<TKey, TValue> rootNode = null;
		private bool exceptionOnNodeKeyClash;
		private int itemCount = 0;
		
		#region Constructors
		
		/// <summary>
		/// Constructor for Gooey binary tree class.
		/// </summary>
		/// <param name="exceptionOnNodeKeyClash">Whether to raise an exception when one tries to set a particular key and a node with that key value already exists.  True do do so, false to just update that node to the new value.</param>
		public GooeyBTree(bool exceptionOnNodeKeyClash) {
			this.exceptionOnNodeKeyClash = exceptionOnNodeKeyClash;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Whether to throw an exception when a new node is added and its key conflicts with the key of an existing node.  If true, throws a GooeyNodeAlreadyExistsException; if false, simply updates that node with the new value.
		/// </summary>
		public bool ExceptionOnNodeKeyClash {
			get { return this.exceptionOnNodeKeyClash; }
			set { this.exceptionOnNodeKeyClash = value; }
		}
		
		#endregion
		
		#region Private methods
		
		/// <summary>
		/// Recursive function that adds a node to the given node if it can, otherwise searches down the tree until it finds a node it can add the node to.
		/// </summary>
		/// <param name="addTo">The node to start the search at, ie. under which we want to add the new node.</param>
		/// <param name="nodeToAdd">The node to add to the tree.</param>
		private void addNodeTo(GooeyBTreeNode<TKey, TValue> addTo, GooeyBTreeNode<TKey, TValue> nodeToAdd) {
			int comparison = addTo.Key.CompareTo(nodeToAdd.Key);
			
			if (comparison > 0) {
				// Node to add's key is smaller than this one...
				if (addTo.LeftNode == null) {
					nodeToAdd.ParentNode = addTo;
					addTo.LeftNode = nodeToAdd;
					this.itemCount++;
				}
				else { this.addNodeTo(addTo.LeftNode, nodeToAdd); }
			}
			else if (comparison < 0) {
				// Node to add's key is larger than this one...
				if (addTo.RightNode == null) {
					nodeToAdd.ParentNode = addTo;
					addTo.RightNode = nodeToAdd;
					this.itemCount++;
				}
				else { this.addNodeTo(addTo.RightNode, nodeToAdd); }
			}
			else {
				// Node to add's key is THE SAME as this one.
				if (this.exceptionOnNodeKeyClash) {
					throw new GooeyNodeAlreadyExistsException();
				}
				else {
					addTo.Value = nodeToAdd.Value;
				}
			}
		}
		
		/// <summary>
		/// Recursive function that gets the node with the 'smallest' key value.
		/// </summary>
		/// <param name="getFrom">The node to start the search at, ie. under which we want to find the smallest value.</param>
		/// <returns>The found node with the smallest value.</returns>
		private GooeyBTreeNode<TKey, TValue> getSmallestNode(GooeyBTreeNode<TKey, TValue> getFrom) {
			if (getFrom == null) { return null; }
			
			if (getFrom.LeftNode == null) { return getFrom; }
			else { return this.getSmallestNode(getFrom.LeftNode); }
		}
		
		/// <summary>
		/// Recursive function that gets the node with the 'largest' key value.
		/// </summary>
		/// <param name="getFrom">The node to start the search at, ie. under which we want to find the largest value.</param>
		/// <returns>The found node with the largest value.</returns>
		private GooeyBTreeNode<TKey, TValue> getLargestNode(GooeyBTreeNode<TKey, TValue> getFrom) {
			if (getFrom == null) { return null; }
			
			if (getFrom.RightNode == null) { return getFrom; }
			else { return this.getLargestNode(getFrom.RightNode); }
		}
		
		/// <summary>
		/// Gets a node in the tree by its key value.
		/// </summary>
		/// <param name="key">The key of the node we're searching for.</param>
		/// <param name="getFrom">The node to start the search at, ie. under which we want to find the node with the given key.</param>
		/// <returns>The found node.  null if the node could not be found.</returns>
		private GooeyBTreeNode<TKey, TValue> getNode(TKey key, GooeyBTreeNode<TKey, TValue> getFrom) {
			if (getFrom == null) { return null; }
			
			int comparison = getFrom.Key.CompareTo(key);
			
			if (comparison > 0) {
				// Node to find's key is smaller than this one...
				return getNode(key, getFrom.LeftNode);
			}
			else if (comparison < 0) {
				// Node to find's key is larger than this one...
				return getNode(key, getFrom.RightNode);
			}
			else {
				// This is the node with the key we're searching for
				return getFrom;
			}
		}
		
		/// <summary>
		/// Deletes the specified note from the tree.
		/// </summary>
		/// <param name="nodeToDelete">The node to delete from the tree.</param>
		private void deleteNode(GooeyBTreeNode<TKey, TValue> nodeToDelete) {
			if (nodeToDelete == null) { throw new ArgumentNullException(); }
			
			bool isRootNode = (nodeToDelete.ParentNode == null ? true : false);
			
			// Do the deletion.
			if (nodeToDelete.LeftNode == null && nodeToDelete.RightNode == null) {
				// Is a leaf node - simply remove it
				if (isRootNode) { this.rootNode = null; }
				else {
					if (nodeToDelete.ParentNode.LeftNode == nodeToDelete) { nodeToDelete.ParentNode.LeftNode = null; }
					else { nodeToDelete.ParentNode.RightNode = null; }
				}
				
				nodeToDelete.ParentNode = null;
				
				this.itemCount--;
			}
			else if (nodeToDelete.LeftNode != null && nodeToDelete.RightNode != null) {
				// Node has both children - find its successor, and replace it with that; then delete the
				// successor from where it was.  Its successor, by the way, is guaranteed to have zero or one
				// children - not two.
				GooeyBTreeNode<TKey, TValue> successorNode = this.GetNextNode(nodeToDelete);
				nodeToDelete.Key = successorNode.Key;
				nodeToDelete.Value = successorNode.Value;
				this.deleteNode(successorNode);
				
				// No need to decrement itemcount here; it will have been decremented by the successor's deleteNode()...
			}
			else {
				// Node has only one child - replace this node with its child
				GooeyBTreeNode<TKey, TValue> childNode;
				if (nodeToDelete.LeftNode == null) { childNode = nodeToDelete.RightNode; }
				else { childNode = nodeToDelete.LeftNode; }
				
				if (isRootNode) { this.rootNode = childNode; }
				else {
					if (nodeToDelete.ParentNode.LeftNode == nodeToDelete) { nodeToDelete.ParentNode.LeftNode = childNode; }
					else { nodeToDelete.ParentNode.RightNode = childNode; }
				}
				
				nodeToDelete.ParentNode = null;
				
				this.itemCount--;
			}
		}
		
		#endregion
		
		#region Public methods
		
		/// <summary>
		/// Adds a node to the tree given a particular key and value for the node to hold.  If a node with an identical key already exists, may throw an exception or overwrite that node, depending on the value of ExceptionOnNodeKeyClash.
		/// </summary>
		/// <param name="key">The new node's key.</param>
		/// <param name="val">The new node's value.</param>
		public void AddNode(TKey key, TValue val) {
			GooeyBTreeNode<TKey, TValue> nodeToAdd = new GooeyBTreeNode<TKey, TValue>();
			nodeToAdd.Key = key;
			nodeToAdd.Value = val;
			
			if (this.rootNode == null) {
				this.rootNode = nodeToAdd;
				this.itemCount++;
			}
			else {
				this.addNodeTo(this.rootNode, nodeToAdd);
			}
		}
		
		public GooeyBTreeNode<TKey, TValue> GetSmallestNode() {
			return this.getSmallestNode(this.rootNode);
		}
		
		public GooeyBTreeNode<TKey, TValue> GetLargestNode() {
			return this.getLargestNode(this.rootNode);
		}
		
		public GooeyBTreeNode<TKey, TValue> GetNextNode(GooeyBTreeNode<TKey, TValue> node) {
			if (node.RightNode == null) {
				GooeyBTreeNode<TKey, TValue> parentNode = node.ParentNode;
				// Only return the parent node if its key is 'larger' than the current node's key, or is
				// null (root node).
				while (parentNode != null && node.Key.CompareTo(parentNode.Key) > 0) { parentNode = parentNode.ParentNode; }
				
				return parentNode;
			}
			else { return this.getSmallestNode(node.RightNode); }
		}
		
		public GooeyBTreeNode<TKey, TValue> GetPreviousNode(GooeyBTreeNode<TKey, TValue> node) {
			if (node.LeftNode == null) {
				GooeyBTreeNode<TKey, TValue> parentNode = node.ParentNode;
				// Only return the parent node if its key is 'smaller' than the current node's key, or is
				// null (root node).
				while (parentNode != null && node.Key.CompareTo(parentNode.Key) < 0) { parentNode = parentNode.ParentNode; }
				
				return parentNode;
			}
			else { return this.getLargestNode(node.LeftNode); }
		}
		
		public GooeyBTreeNode<TKey, TValue> GetNode(TKey nodeKey) {
			return this.getNode(nodeKey, this.rootNode);
		}
		
		/// <summary>
		/// Deletes a node from the tree with the given key.
		/// </summary>
		/// <param name="nodeKey">The key of the node to delete.</param>
		/// <returns>True if a node with the given key existed and was deleted; false if no such node existed.</returns>
		public bool DeleteNode(TKey nodeKey) {
			GooeyBTreeNode<TKey, TValue> node = this.GetNode(nodeKey);
			if (node == null) { return false; }
			
			this.deleteNode(node);
			return true;
		}
		
		/// <summary>
		/// Completely clears/terminates the binary tree of all data.
		/// </summary>
		public void DeleteAllNodes() {
			GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
			List<TKey> allNodeKeys = new List<TKey>();
			
			while (node != null) {
				allNodeKeys.Add(node.Key);
				node = this.GetNextNode(node);
			}
			
			foreach (TKey nodeToDeleteKey in allNodeKeys) {
				this.DeleteNode(nodeToDeleteKey);
			}
		}
		
		#endregion
		
		#region IDictionary<TKey,TValue> Members
		
		public void Add(TKey key, TValue value) {
			this.AddNode(key, value);
		}
		
		public bool ContainsKey(TKey key) {
			return (this.GetNode(key) != null);
		}
		
		public ICollection<TKey> Keys {
			get {
				List<TKey> keys = new List<TKey>();
				
				GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
				
				while (node != null) {
					keys.Add(node.Key);
					node = this.GetNextNode(node);
				}
				
				return keys;
			}
		}
		
		public bool Remove(TKey key) {
			return this.DeleteNode(key);
		}
		
		public bool TryGetValue(TKey key, out TValue value) {
			GooeyBTreeNode<TKey, TValue> node = this.GetNode(key);
			if (node != null) { value = node.Value; return true; }
			else { value = default(TValue); return false; }
		}
		
		public ICollection<TValue> Values {
			get {
				List<TValue> values = new List<TValue>();
				
				GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
				
				while (node != null) {
					values.Add(node.Value);
					node = this.GetNextNode(node);
				}
				
				return values;
			}
		}
		
		public TValue this[TKey key] {
			get {
				GooeyBTreeNode<TKey, TValue> node = this.GetNode(key);
				if (node != null) { return node.Value; }
				else { return default(TValue); }
			}
			set { this.AddNode(key, value); }
		}
		
		#endregion
		
		#region ICollection<KeyValuePair<TKey,TValue>> Members
		
		public void Add(KeyValuePair<TKey, TValue> item) {
			this.AddNode(item.Key, item.Value);
		}
		
		public void Clear() {
			this.DeleteAllNodes();
		}
		
		public bool Contains(KeyValuePair<TKey, TValue> item) {
			GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
			
			while (node != null) {
				if (node.Key.CompareTo(item.Key) == 0 && node.Value.Equals(item.Value)) { return true; }
				node = this.GetNextNode(node);
			}
			
			return false;
		}
		
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			// 'Copies the elements of the ICollection to an Array, starting at a particular Array index.'
			if (array == null) { throw new ArgumentNullException(); }
			if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(); }
			if (array.Rank > 1) { throw new ArgumentException(); }
			if (arrayIndex >= array.Length) { throw new ArgumentException(); }
			if (this.itemCount > (arrayIndex - array.Length)) { throw new ArgumentException(); }
			
			// Do the copying.
			GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
			
			while (node != null) {
				array[arrayIndex++] = new KeyValuePair<TKey,TValue>(node.Key, node.Value);
				node = this.GetNextNode(node);
			}
		}
		
		public int Count {
			get { return this.itemCount; }
		}
		
		public bool IsReadOnly {
			get { return false; }
		}
		
		public bool Remove(KeyValuePair<TKey, TValue> item) {
			GooeyBTreeNode<TKey, TValue> node = this.GetSmallestNode();
			
			while (node != null) {
				if (node.Key.CompareTo(item.Key) == 0 && node.Value.Equals(item.Value)) { this.deleteNode(node); return true; }
				node = this.GetNextNode(node);
			}
			
			return false;
		}
		
		#endregion
		
		#region IEnumerable<KeyValuePair<TKey,TValue>> Members
		
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			return new GooeyBTreeEnumerator(this);
		}
		
		#endregion
		
		#region IEnumerable Members
		
		// Explicit, as it's less useful.  Can't be public because it's explicit.  If you want to access it you
		// must cast the GooeyBTree object to a System.Collections.IEnumerable.
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return new GooeyBTreeEnumerator(this);
		}
		
		#endregion
		
		private class GooeyBTreeEnumerator : IEnumerator<KeyValuePair<TKey, TValue>> {
			private GooeyBTree<TKey, TValue> treeToTraverse;
			private GooeyBTreeNode<TKey, TValue> currentEnumeratorNode = null;
			private bool enumeratorIsInResetPosition = true;
			
			#region Constructors
			
			public GooeyBTreeEnumerator(GooeyBTree<TKey, TValue> treeToTraverse) {
				this.treeToTraverse = treeToTraverse;
			}
			
			#endregion
			
			#region IDisposable Members
			
			public void Dispose() { }
			
			#endregion
			
			#region IEnumerator<KeyValuePair<TKey,TValue>> Members
			
			public KeyValuePair<TKey, TValue> Current {
				get {
					if (this.currentEnumeratorNode == null) { return new KeyValuePair<TKey,TValue>(); }
					else {
						return new KeyValuePair<TKey, TValue>(this.currentEnumeratorNode.Key, this.currentEnumeratorNode.Value);
					}
				}
			}
			
			#endregion
			
			#region IEnumerator Members
			
			object IEnumerator.Current {
				get {
					if (this.currentEnumeratorNode == null) { return null; }
					else {
						return new KeyValuePair<TKey, TValue>(this.currentEnumeratorNode.Key, this.currentEnumeratorNode.Value);
					}
				}
			}
			
			public bool MoveNext() {
				if (this.enumeratorIsInResetPosition) {
					this.currentEnumeratorNode = this.treeToTraverse.GetSmallestNode();
					this.enumeratorIsInResetPosition = false;
				}
				else {
					if (this.currentEnumeratorNode != null) {
						this.currentEnumeratorNode = this.treeToTraverse.GetNextNode(this.currentEnumeratorNode);
					}
				}
				
				return (this.currentEnumeratorNode != null);
			}
			
			public void Reset() {
				// Remember, this resets the enumerator to *BEFORE* the first node.
				this.currentEnumeratorNode = null;
				this.enumeratorIsInResetPosition = true;
			}
			
			#endregion
		}
	}
	
	public class GooeyBTreeNode<TKey, TValue> {
		/// <summary>
		/// Used to point to the node whose key is a 'lesser' value than this one.
		/// </summary>
		public GooeyBTreeNode<TKey, TValue> LeftNode { get; set; }
		/// <summary>
		/// Used to point to the node whose key is a 'greater' value than this one.
		/// </summary>
		public GooeyBTreeNode<TKey, TValue> RightNode { get; set; }
		/// <summary>
		/// Used to point to the node that, in the tree, is the parent of this one.
		/// </summary>
		public GooeyBTreeNode<TKey, TValue> ParentNode { get; set; }
		
		/// <summary>
		/// This node's key.
		/// </summary>
		public TKey Key { get; set; }
		/// <summary>
		/// This node's value.
		/// </summary>
		public TValue Value { get; set; }
	}
	
	#region Exceptions
	
	public class GooeyNodeAlreadyExistsException : Exception {
		#region Private vars
		
		private const string defaultMsg = "The node with this key already exists.";
		// ^ As any 'const' is made a compile-time constant, this text will obviously be
		// available to the constructors before the object has been instantiated, as is
		// necessary.
		
		#endregion
		
		#region Constructors
		// Note that the 'public ClassName(...): base() {' notation is explicitly telling
		// the compiler to call this class's base class's empty constructor.  A constructor
		// HAS to call either a base() or this() constructor before its own body, and the
		// '(...): base()' notation (with the colon) is the way to do it explicitly.  If you
		// don't use this notation, base() will be implicitly called.  Therefore, this:
		// public ClassName(...) {...}
		// is identical to this:
		// public ClassName(...): base() {...}
		// 
		// For more information, see:
		// http://msdn2.microsoft.com/en-us/library/aa645603.aspx
		// http://www.jaggersoft.com/csharp_standard/17.10.1.htm
		
		public GooeyNodeAlreadyExistsException(): base(defaultMsg) {
			// No further implementation
		}
		
		public GooeyNodeAlreadyExistsException(string message): base(message) {
			// No further implementation
		}
		
		public GooeyNodeAlreadyExistsException(string message, Exception innerException): base(message, innerException) {
			// No further implementation
		}
		
		protected GooeyNodeAlreadyExistsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context): base(info, context) {
			// No further implementation
		}
		
		#endregion
	}
	
	#endregion
}
