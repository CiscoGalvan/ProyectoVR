using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using Project.Scripts.Weapon;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;

namespace Project.Scripts.Fractures
{
	public class ChunkNode : MonoBehaviour
	{
		public HashSet<ChunkNode> Neighbours = new HashSet<ChunkNode>();
		public ChunkNode[] NeighboursArray = new ChunkNode[0];
		private Dictionary<Joint, ChunkNode> JointToChunk = new Dictionary<Joint, ChunkNode>();
		private Dictionary<ChunkNode, Joint> ChunkToJoint = new Dictionary<ChunkNode, Joint>();
		private Rigidbody rb;
		private Vector3 frozenPos;
		private Quaternion forzenRot;
		private bool frozen;
		private bool launched = false;
		private Transform trans;
		private float actualTime = 0;
		private float collisionTime = 0;

		bool r = false;
		public bool IsStatic => rb.isKinematic;
		public Color Color { get; set; } = Color.black;
		public bool HasBrokenLinks { get; private set; }


		float delay = 0.1f;
		private XRGrabInteractable grabComponent;
		private bool Contains(ChunkNode chunkNode)
		{
			return Neighbours.Contains(chunkNode);
		}

		
		public void Setup()
		{

			trans = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>();


			Freeze();


			JointToChunk.Clear();
			ChunkToJoint.Clear();
			foreach (var joint in GetComponents<Joint>())
			{
				var chunk = joint.connectedBody.GetOrAddComponent<ChunkNode>();
				JointToChunk[joint] = chunk;
				ChunkToJoint[chunk] = joint;
			}

			foreach (var chunkNode in ChunkToJoint.Keys)
			{
				Neighbours.Add(chunkNode);

				if (chunkNode.Contains(this) == false)
				{
					chunkNode.Neighbours.Add(this);
				}
			}

			NeighboursArray = Neighbours.ToArray();
		}
	
		private void OnJointBreak(float breakForce)
		{
			//print(breakForce);
			//falll();
			//HasBrokenLinks = true;
		}

		public void CleanBrokenLinks()
		{
			var brokenLinks = JointToChunk.Keys.Where(j => j == false).ToList();
			foreach (var link in brokenLinks)
			{
				var body = JointToChunk[link];

				JointToChunk.Remove(link);
				ChunkToJoint.Remove(body);

				body.Remove(this);
				Neighbours.Remove(body);
			}

			NeighboursArray = Neighbours.ToArray();
			HasBrokenLinks = false;
		}

		private void Remove(ChunkNode chunkNode)
		{
			ChunkToJoint.Remove(chunkNode);
			Neighbours.Remove(chunkNode);
			NeighboursArray = Neighbours.ToArray();
		}

		public void Unfreeze()
		{
			if (GetComponent<AudioSource>() == null) return;
			falll();
			GetComponent<AudioSource>().volume = 0.1f;
			GetComponent<AudioSource>().Play();
			for (int i = 0; i < trans.childCount - 1; i++)
			{
				GameObject g = trans.GetChild(i).gameObject;
				ChunkNode a = g.GetComponent<ChunkNode>();
				//Descomentar
				a.falll();
			}
			while (trans.childCount !=0)
			{
				trans.GetChild(0).SetParent(null);
			}

			Destroy(this.gameObject);
		}
		public void falll()
		{
			frozen = false;
			rb.constraints = RigidbodyConstraints.None;
			rb.useGravity = true;
			rb.gameObject.layer = LayerMask.NameToLayer("Default");
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
			rb.isKinematic = false;
			
		}

		private void Freeze()
		{

			frozen = true;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			rb.useGravity = false;
			rb.gameObject.layer = LayerMask.NameToLayer("FrozenChunks");

		}

		private void OnDrawGizmos()
		{
			var worldCenterOfMass = transform.TransformPoint(transform.GetComponent<Rigidbody>().centerOfMass);

			if (IsStatic)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(worldCenterOfMass, 0.05f);
			}
			else
			{
				Gizmos.color = Color.SetAlpha(0.5f);
				Gizmos.DrawSphere(worldCenterOfMass, 0.1f);
			}

			foreach (var joint in JointToChunk.Keys)
			{
				if (joint)
				{
					var from = transform.TransformPoint(rb.centerOfMass);
					var to = joint.connectedBody.transform.TransformPoint(joint.connectedBody.centerOfMass);
					Gizmos.color = Color;
					Gizmos.DrawLine(from, to);
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
			foreach (var node in Neighbours)
			{
				var mesh = node.GetComponent<MeshFilter>().mesh;
				Gizmos.color = Color.yellow.SetAlpha(.2f);
				Gizmos.DrawMesh(mesh, node.transform.position, node.transform.rotation);
			}
		}
	}
}