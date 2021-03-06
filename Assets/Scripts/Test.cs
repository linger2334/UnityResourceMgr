﻿using UnityEngine;
using System.Collections;
using Utils;

public class Test : CachedMonoBehaviour {

	void OnGUI()
	{
		if (GameMgr.Instance.IsConfigLoaded)
		{
			DrawButtons();
			DrawSpirtesButtons();
		}
	}

	void DrawSpirtesButtons()
	{
		if (!m_IsSpritesLoading)
		{
			if (m_SpriteList != null)
			{
				if (GUI.Button(new Rect(100, 220, 150, 50), "删除Sprites"))
				{
					ResourceMgr.Instance.DestroySprites(m_SpriteList, true);
					m_IsSpritesLoading = false;
					m_SpriteList = null;
				}
			} else
			if (GUI.Button(new Rect(100, 220, 150, 50), "(异步)读取Sprites"))
			{
				m_IsSpritesLoading = true;
				if (!ResourceMgr.Instance.LoadSpritesAsync("resources/@spirtes/spirtes.png",
				  delegate (float process, bool isDone, Object[] objs) {
					if (isDone)
					{
						m_IsSpritesLoading = false;
						if (m_SpriteList != null)
						{
							ResourceMgr.Instance.DestroySprites(m_SpriteList, true);
							m_SpriteList = null;
						}
						if (objs != null && objs.Length > 0)
						{
							m_SpriteList = new Sprite[objs.Length];
							for (int i = 0; i < objs.Length; ++i)
							{
								m_SpriteList[i] = objs[i] as Sprite;
							}
						}
					}
				}))
					m_IsSpritesLoading = false;
			}
		}
	}
	
	void DrawButtons()
	{
		if (GUI.Button(new Rect(100, 100, 150, 50), "(同步)切换场景"))
		{
			ChangeScene(false);
		}

		if (GUI.Button(new Rect(260, 100, 150, 50), "(异步)切换场景"))
		{
			ChangeScene(true);
		}
			

			if (GUI.Button(new Rect(260, 160, 150, 50), "(异步)创建Prefab物体"))
			{
				ResourceMgr.Instance.CreateGameObjectAsync("resources/@prefab/cube.prefab", null);
				ResourceMgr.Instance.CreateGameObjectAsync("resources/@prefab/flag.prefab", null);
			}
	}

	void ChangeScene(bool isAsync)
	{
		if (!string.IsNullOrEmpty(m_CurScene))
			ResourceMgr.Instance.CloseScene(m_CurScene);
		m_CurScene = "1";
		if (isAsync)
			ResourceMgr.Instance.LoadSceneAsync(m_CurScene, false, null);
		else
			ResourceMgr.Instance.LoadScene(m_CurScene, false);

	}
		
	private string m_CurScene = string.Empty;
	private bool m_IsSpritesLoading = false;
	private Sprite[] m_SpriteList = null;
}
