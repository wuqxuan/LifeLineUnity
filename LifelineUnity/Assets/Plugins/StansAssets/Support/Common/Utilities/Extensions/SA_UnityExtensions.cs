////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

//namespace SA.Common.Extensions {

	public static class SA_UnityExtensions  {


		public static void MoveTo(this GameObject go, Vector3 position, float time, SA.Common.Animation.EaseType easeType = SA.Common.Animation.EaseType.linear, System.Action OnCompleteAction = null ) {
			SA.Common.Animation.ValuesTween tw = go.AddComponent<SA.Common.Animation.ValuesTween>();

			tw.DestoryGameObjectOnComplete = false;
			tw.VectorTo(go.transform.position, position, time, easeType);

			tw.OnComplete += OnCompleteAction;
		}


		public static void ScaleTo(this GameObject go, Vector3 scale, float time, SA.Common.Animation.EaseType easeType = SA.Common.Animation.EaseType.linear, System.Action OnCompleteAction = null ) {
			SA.Common.Animation.ValuesTween tw = go.AddComponent<SA.Common.Animation.ValuesTween>();

			tw.DestoryGameObjectOnComplete = false;
			tw.ScaleTo(go.transform.localScale, scale, time, easeType);

			tw.OnComplete += OnCompleteAction;
		}


		public static Sprite ToSprite(this Texture2D texture) {
			return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f)); 
		}
			

	}

//}
