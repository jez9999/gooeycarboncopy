using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;

namespace GooeyControls
{
	public class GooeyTree : TreeView {
		#region Private vars
		
		/// <summary>
		/// The Treeview's .StateImageList controls what checkbox images get displayed.  If it is null, as it is
		/// if the .CheckBoxes property is set to false, no checkbox images get displayed.  However, conversely,
		/// we DO want .CheckBoxes set to false because this also means that the TreeView doesn't try to manage
		/// checkboxes itself; we're still able to manipulate this image list to display our own custom
		/// checkboxes.  We'll set .StateImageList to our custom state image list, which will have the images we
		/// want for our custom checkbox icons.
		/// </summary>
		private ImageList customStateImageList = null;
		private bool checkBoxes = true;
		
		public class GooeyTreeTagData {
			public GooeyTree.CheckState CheckState { get; set; }
			public Object OtherTagInfo { get; set; }
		}
		
		// These are the REAL values that a checkbox can have, that will be reported back to the calling code.
		public enum CheckState {
			Unchecked,
			Checked,
			PermRecurse,
		}
		
		// These values MUST correspond to the order we add the custom state images.
		private enum CheckImageState : int {
			UncheckedNormal = 0,
			CheckedNormal = 1,
			UncheckedMixed = 2,
			CheckedMixed = 3,
			UncheckedDisabled = 4,
			CheckedDisabled = 5,
			PermRecurse = 6,
		}
		
		private enum CustomCheckImageState {
			PermRecurse = 0,
		}
		
		private enum CheckBoxSpecialOperation {
			None,
			CheckRecursive,
			CheckPermRecursive,
		}
		
		#endregion
		
		#region Public vars
		
		[DefaultValue(true)]
		public new bool CheckBoxes {
			// As was mentioned above, we conversely want to set CheckBoxes to false when the user tells us to
			// enable checkboxes as that lets us manage checkboxes ourselves.  We'll store whether we want
			// to implement OUR custom checkbox functionality internally.
			get {
				return this.checkBoxes;
			}
			set {
				this.checkBoxes = value;
				initStateImageList(this.CheckBoxes);
			}
		}
		
		#endregion
		
		#region Overriding TreeView
		
		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);
			
			// As was mentioned above, we always want TreeView's CheckBoxes set to false.
			base.CheckBoxes = false;
			
			if (this.customStateImageList == null) {
				// Init internal state image list
				this.customStateImageList = new ImageList();
				using (Graphics gfx = base.CreateGraphics()) {
					Size glyphSize = CheckBoxRenderer.GetGlyphSize(gfx, CheckBoxState.UncheckedNormal);
					
					// The order we add these MUST correspond to the order in the CheckImageState enum.
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.UncheckedNormal, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.CheckedNormal, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.UncheckedPressed, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.CheckedPressed, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.UncheckedDisabled, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(CheckBoxState.CheckedDisabled, null, glyphSize));
					this.customStateImageList.Images.Add(getStateImage(null, CustomCheckImageState.PermRecurse, glyphSize));
				}
			}
			
			initStateImageList(this.CheckBoxes);
			initNodesState(base.Nodes);
		}
		
		protected override void OnMouseDown(MouseEventArgs ea) {
			base.OnMouseDown(ea);
			
			if (ea.Button == MouseButtons.Left) {
				TreeViewHitTestInfo info = base.HitTest(ea.Location);
				if (info.Node != null && info.Location == TreeViewHitTestLocations.StateImage) {
					// The modifier keys value could contain Shift | Control | Alt...
					if ((Control.ModifierKeys & Keys.Control) > 0) {
						this.nodeActivated(info.Node, CheckBoxSpecialOperation.CheckPermRecursive, false);
					}
					else if ((Control.ModifierKeys & Keys.Shift) > 0) {
						this.nodeActivated(info.Node, CheckBoxSpecialOperation.CheckRecursive, false);
					}
					else {
						this.nodeActivated(info.Node, CheckBoxSpecialOperation.None, false);
					}
				}
			}
		}
		
		protected override void OnKeyDown(KeyEventArgs ea) {
			base.OnKeyDown(ea);
			
			if (ea.KeyCode == Keys.Space) {
				if (base.SelectedNode != null) {
					// The modifier keys value could contain Shift | Control | Alt...
					if ((Control.ModifierKeys & Keys.Control) > 0) {
						this.nodeActivated(base.SelectedNode, CheckBoxSpecialOperation.CheckPermRecursive, true);
					}
					else if ((Control.ModifierKeys & Keys.Shift) > 0) {
						this.nodeActivated(base.SelectedNode, CheckBoxSpecialOperation.CheckRecursive, true);
					}
					else {
						this.nodeActivated(base.SelectedNode, CheckBoxSpecialOperation.None, true);
					}
				}
			}
		}
		
		#endregion
		
		#region Private methods
		
		/// <summary>
		/// Recursively initialize the state image and tag data of all nodes in the collection passed.
		/// </summary>
		/// <param name="nodes">The tree node collection to init the state of the nodes in.</param>
		private static void initNodesState(TreeNodeCollection nodes) {
			foreach (TreeNode node in nodes) {
				node.StateImageIndex = (int)CheckImageState.UncheckedNormal;
				node.Tag = new GooeyTree.GooeyTreeTagData { CheckState = CheckState.Unchecked };
				
				// Recursively init children
				if (node.Nodes.Count != 0) {
					initNodesState(node.Nodes);
				}
			}
		}
		
		/// <summary>
		/// Initialize the state image list of the TreeView.
		/// </summary>
		/// <param name="enable">Whether to enable checkbox images for this TreeView or not.</param>
		private void initStateImageList(bool enable) {
			if (enable) { base.StateImageList = this.customStateImageList; }
			else { base.StateImageList = null; }
		}
		
		private static Image getStateImage(CheckBoxState? state, CustomCheckImageState? customState, Size imageSize) {
			if (
				(state != null && customState != null) ||
				(state == null && customState == null)
			) {
				throw new Exception("Only one of state and customState may be passed!");
			}
			
			bool renderCustomState = false;
			
			if (state == null) { renderCustomState = true; }
			
			if (renderCustomState) {
				switch (customState) {
					case CustomCheckImageState.PermRecurse:
					state = CheckBoxState.CheckedNormal;
					break;
				}
			}
			
			// 16x16 is the size of the area where the checkbox glyph will go
			Image bmp = new Bitmap(16, 16);
			using (Graphics gfx = Graphics.FromImage(bmp)) {
				Point pt = new Point((16 - imageSize.Width) / 2, (16 - imageSize.Height) / 2);
				CheckBoxRenderer.DrawCheckBox(gfx, pt, (CheckBoxState)state);
				
				if (renderCustomState) {
					switch (customState) {
						case CustomCheckImageState.PermRecurse:
						// Draw red border to indicate 'permenantly recursive' node
						gfx.DrawRectangle(new Pen(Brushes.Red, 2), 2, 2, imageSize.Width - 2, imageSize.Height - 2);
						break;
					}
				}
			}
			
			return bmp;
		}
		
		/// <summary>
		/// To be called when a checkbox node is activated either by key or by mouse.
		/// </summary>
		/// <param name="node">The checkbox node that was activated.</param>
		/// <param name="op">The special operation (if any) to perform on the node, with regards to checking its child nodes.</param>
		/// <param name="activatedByKeyboard">If the checkbox node was activated by keyboard, true; otherwise false.</param>
		private void nodeActivated(TreeNode node, CheckBoxSpecialOperation op, bool activatedByKeyboard) {
			bool dontUpdateChildren = false;
			bool dontUpdateParents = false;
			bool treatPermRecursiveAsChecked = false;
			
			GooeyTree.GooeyTreeTagData tagData = (GooeyTree.GooeyTreeTagData)node.Tag;
			switch (node.StateImageIndex) {
				case (int)CheckImageState.UncheckedNormal:
				if (op == CheckBoxSpecialOperation.None) {
					// Check this node (to state unmixed).
					node.StateImageIndex = (int)CheckImageState.CheckedNormal;
					tagData.CheckState = CheckState.Checked;
					dontUpdateChildren = true;
				}
				else if (op == CheckBoxSpecialOperation.CheckRecursive) {
					// Check this node and all child nodes.
					if (node.Nodes.Count > 0) { node.StateImageIndex = (int)CheckImageState.CheckedMixed; }
					else { node.StateImageIndex = (int)CheckImageState.CheckedNormal; }
					tagData.CheckState = CheckState.Checked;
				}
				else if (op == CheckBoxSpecialOperation.CheckPermRecursive) {
					// Make this node perm recursive, and all child nodes checked-disabled.
					node.StateImageIndex = (int)CheckImageState.PermRecurse;
					tagData.CheckState = CheckState.PermRecurse;
					treatPermRecursiveAsChecked = true;
				}
				break;
				
				case (int)CheckImageState.CheckedNormal:
				// Always want to uncheck this node (to state unmixed).
				node.StateImageIndex = (int)CheckImageState.UncheckedNormal;
				tagData.CheckState = CheckState.Unchecked;
				dontUpdateChildren = true;
				break;
				
				case (int)CheckImageState.UncheckedMixed:
				if (op == CheckBoxSpecialOperation.None) {
					// Check this node (to state mixed).
					node.StateImageIndex = (int)CheckImageState.CheckedMixed;
					tagData.CheckState = CheckState.Checked;
					dontUpdateChildren = true;
				}
				else if (op == CheckBoxSpecialOperation.CheckRecursive) {
					// Check this node and all child nodes which are not perm recursive or disabled.
					node.StateImageIndex = (int)CheckImageState.CheckedMixed;
					tagData.CheckState = CheckState.Checked;
				}
				else if (op == CheckBoxSpecialOperation.CheckPermRecursive) {
					// Make this node perm recursive, and all child nodes checked-disabled.
					node.StateImageIndex = (int)CheckImageState.PermRecurse;
					tagData.CheckState = CheckState.PermRecurse;
					treatPermRecursiveAsChecked = true;
				}
				break;
				
				case (int)CheckImageState.CheckedMixed:
				if (op == CheckBoxSpecialOperation.None) {
					// Uncheck this node (to state mixed).
					node.StateImageIndex = (int)CheckImageState.UncheckedMixed;
					tagData.CheckState = CheckState.Unchecked;
					dontUpdateChildren = true;
				}
				else if (op == CheckBoxSpecialOperation.CheckRecursive) {
					// Uncheck this node and all child nodes which are not perm recursive or disabled.
					if (aChildIsPermRecursive(node)) { node.StateImageIndex = (int)CheckImageState.UncheckedMixed; }
					else { node.StateImageIndex = (int)CheckImageState.UncheckedNormal; }
					tagData.CheckState = CheckState.Unchecked;
				}
				else if (op == CheckBoxSpecialOperation.CheckPermRecursive) {
					// Uncheck this node and ALL child nodes.
					node.StateImageIndex = (int)CheckImageState.UncheckedNormal;
					tagData.CheckState = CheckState.Unchecked;
					treatPermRecursiveAsChecked = true;
				}
				break;
				
				case (int)CheckImageState.UncheckedDisabled:
				case (int)CheckImageState.CheckedDisabled:
				// Always want to do nothing.
				dontUpdateChildren = true;
				dontUpdateParents = true;
				break;
				
				case (int)CheckImageState.PermRecurse:
				// Always want to uncheck this node and ALL child nodes.
				node.StateImageIndex = (int)CheckImageState.UncheckedNormal;
				tagData.CheckState = CheckState.Unchecked;
				treatPermRecursiveAsChecked = true;
				break;
			}
			
			if (!dontUpdateChildren) { updateChildren(node, treatPermRecursiveAsChecked); }
			if (!dontUpdateParents) { updateParents(node); }
			
			if (!dontUpdateChildren || !dontUpdateParents) { base.OnAfterCheck(new TreeViewEventArgs(node, (activatedByKeyboard ? TreeViewAction.ByKeyboard : TreeViewAction.ByMouse))); }
		}
		
		/// <summary>
		/// Recursively update the specified node's children according to its value.
		/// </summary>
		/// <param name="parent">The node whose children to update.</param>
		/// <param name="treatPermRecursiveAsChecked">Whether to treat perm recursive and disabled checked nodes as regular checked nodes, as normally they would be immune from recursive unchecking.</param>
		private static void updateChildren(TreeNode parent, bool treatPermRecursiveAsChecked) {
			foreach (TreeNode node in parent.Nodes) {
				GooeyTree.GooeyTreeTagData tagData = (GooeyTree.GooeyTreeTagData)node.Tag;
				
				int imgIndex = node.StateImageIndex;
				if (imgIndex == (int)CheckImageState.PermRecurse || imgIndex == (int)CheckImageState.CheckedDisabled) {
					if (treatPermRecursiveAsChecked) { imgIndex = (int)CheckImageState.CheckedNormal; }
				}
				
				// Never update perm recursives...
				if (imgIndex != (int)CheckImageState.PermRecurse) {
					if (parent.StateImageIndex == (int)CheckImageState.PermRecurse || parent.StateImageIndex == (int)CheckImageState.CheckedDisabled) {
						node.StateImageIndex = (int)CheckImageState.CheckedDisabled;
						// Although we're displaying these as checked-disabled, logically their being checked is
						// just a function of a parent perm recursive node's being checked.  For that reason, it's
						// more convenient to actually think of these nodes as unchecked.
						tagData.CheckState = CheckState.Unchecked;
					}
					else if (parent.StateImageIndex == (int)CheckImageState.CheckedNormal || parent.StateImageIndex == (int)CheckImageState.CheckedMixed) {
						if (node.Nodes.Count == 0) { node.StateImageIndex = (int)CheckImageState.CheckedNormal; }
						else { node.StateImageIndex = (int)CheckImageState.CheckedMixed; }
						tagData.CheckState = CheckState.Checked;
					}
					else if (parent.StateImageIndex == (int)CheckImageState.UncheckedNormal || parent.StateImageIndex == (int)CheckImageState.UncheckedMixed) {
						if (!treatPermRecursiveAsChecked && aChildIsPermRecursive(node)) { node.StateImageIndex = (int)CheckImageState.UncheckedMixed; }
						else { node.StateImageIndex = (int)CheckImageState.UncheckedNormal; }
						tagData.CheckState = CheckState.Unchecked;
					}
					
					// Recursively update children
					updateChildren(node, treatPermRecursiveAsChecked);
				}
			}
		}
		
		/// <summary>
		/// Recursively update the specified node's parents according to its value.
		/// </summary>
		/// <param name="child">The node whose parents to update.</param>
		private static void updateParents(TreeNode child) {
			// This should never be called where a parent is a perm recursive, as all children of such should be
			// checked-disabled.
			TreeNode parent = child.Parent;
			
			if (parent != null) {
				bool aChildIsChecked = false;
				foreach (TreeNode node in parent.Nodes) {
					if (node.StateImageIndex != (int)CheckImageState.UncheckedNormal) {
						aChildIsChecked = true;
						break;
					}
				}
				
				if (parent.StateImageIndex == (int)CheckImageState.CheckedNormal || parent.StateImageIndex == (int)CheckImageState.CheckedMixed) {
					if (aChildIsChecked) { parent.StateImageIndex = (int)CheckImageState.CheckedMixed; }
					else { parent.StateImageIndex = (int)CheckImageState.CheckedNormal; }
				}
				else if (parent.StateImageIndex == (int)CheckImageState.UncheckedNormal || parent.StateImageIndex == (int)CheckImageState.UncheckedMixed) {
					if (aChildIsChecked) { parent.StateImageIndex = (int)CheckImageState.UncheckedMixed; }
					else { parent.StateImageIndex = (int)CheckImageState.UncheckedNormal; }
				}
				
				// Recursively update parents
				updateParents(parent);
			}
		}
		
		/// <summary>
		/// Recursively checks a node's children to see whether one is perm recursive.
		/// </summary>
		/// <param name="parent">The node whose children to check.</param>
		/// <returns>If a child is perm recursive, true; otherwise false.</returns>
		private static bool aChildIsPermRecursive(TreeNode parent) {
			foreach (TreeNode node in parent.Nodes) {
				if (node.StateImageIndex == (int)CheckImageState.PermRecurse) { return true; }
				
				// Recursively check children
				if (aChildIsPermRecursive(node)) { return true; }
			}
			
			return false;
		}
		
		#endregion
	}
}
