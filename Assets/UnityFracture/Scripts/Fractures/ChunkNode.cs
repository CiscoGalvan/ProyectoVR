using System.Collections.Generic;
using System.Linq;
using Project.Scripts.Utils;
using Project.Scripts.Weapon;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

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
		private XRGrabInteractable grabCmmponent;
		private XRGeneralGrabTransformer grabTrans;
		bool r = false;
		public bool IsStatic => rb.isKinematic;
		public Color Color { get; set; } = Color.black;
		public bool HasBrokenLinks { get; private set; }

		InteractionLayerMask layerMask;
		float launchedTime;
		private bool Contains(ChunkNode chunkNode)
		{
			return Neighbours.Contains(chunkNode);
		}

		private void Update()
		{
			if (launched && trans.childCount != 0 && launchedTime + Time.deltaTime < Time.realtimeSinceStartup) 
			{
				Vector3 vel = (GetComponent<Rigidbody>().velocity);
				while (trans.childCount != 1)
				{
					GameObject child = trans.GetChild(0).gameObject;
					child.transform.SetParent(null);
					AudioClip clip = Resources.Load<AudioClip>("a");
					if (child.GetComponent<Rigidbody>() != null)
					{
						child.GetComponent<Rigidbody>().velocity = vel;
					

						var audio = child.AddComponent<AudioSource>();
						audio.clip = clip;
						audio.volume = 0.1f;
						audio.playOnAwake = true;

						
					}
					
				}
				Destroy(this.gameObject,2);
			}
		}
		public void Setup()
		{

			trans = GetComponent<Transform>();
			rb = GetComponent<Rigidbody>(); rb.isKinematic = false;

			if (trans.childCount != 0)
			{

				grabCmmponent = GetComponent<XRGrabInteractable>();
				if(grabCmmponent != null) {
					layerMask = grabCmmponent.interactionLayers;
					grabTrans = GetComponent<XRGeneralGrabTransformer>();
				}
			}
			
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
			if (this.trans.parent != null && breakForce > 1000)
			{
				this.trans.parent.GetComponent<ChunkNode>().Unfreeze();
			}
			else if (breakForce > 1000) falll();

			if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play();

			HasBrokenLinks = true;
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
			launched = true;
			launchedTime = Time.realtimeSinceStartup;
		    GetComponent<AudioSource>().Play();
			

			for (int i = 0; i < trans.childCount - 1; i++)
			{
				GameObject g = trans.GetChild(i).gameObject;
				ChunkNode a = g.GetComponent<ChunkNode>();
				//Descomentar
				a.falll();
			}

			
			//Destroy(this.gameObject);
		}
	
		public void falll()
		{
			frozen = false;
			rb.constraints = RigidbodyConstraints.None;
			rb.useGravity = true;
			rb.gameObject.layer = LayerMask.NameToLayer("Default");
			rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		
			
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