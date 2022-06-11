using Shadout.Controllers;
using Shadout.Utilities;
using UnityEngine;

namespace Shadout.Models
{
	public class ContenderManager : Singleton<ContenderManager>
	{
		#region SerializedFields

		[SerializeField]
		private ContenderBase[] contenders;

		#endregion

		#region Variables

		#endregion

		#region Events

		#endregion

		#region Props

		#endregion

		#region Unity Methods

		#endregion

		#region Methods

		public void Init()
		{
			for (int i = 0; i < contenders.Length; i++)
			{
				contenders[i].InitContender();
			}
		}

		#endregion

		#region Callbacks

		#endregion
	}
}