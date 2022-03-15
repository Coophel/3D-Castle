using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    Dictionary<Renderer, Material[]> glowMaterialDictionary = new Dictionary<Renderer, Material[]>();
	Dictionary<Renderer, Material[]> originalMaterialDictionary = new Dictionary<Renderer, Material[]>();

	Dictionary<Color, Material> cachedGlowMaterials = new Dictionary<Color, Material>();

	public Material glowmaterial;

	private bool isGlowing = false;

#region Unity Functions
	private void Awake()
	{
		PrepareMaterialDictionaries();
	}

#endregion

#region Public Functions
	public void ToggleGlow()
	{
		if (isGlowing == false)
		{
			foreach (Renderer renderer in originalMaterialDictionary.Keys)
			{
				renderer.materials = glowMaterialDictionary[renderer];
			}
		}
		else
		{
			foreach (Renderer renderer in originalMaterialDictionary.Keys)
			{
				renderer.materials = originalMaterialDictionary[renderer];
			}
		}
		isGlowing = !isGlowing;
	}

	public void ToggleGlow(bool state)
	{
		if (isGlowing == state)
			return ;
		isGlowing = !state;
		ToggleGlow();
	}
#endregion

#region Private Functions
	private void PrepareMaterialDictionaries()
	{
		// 선택된 모든 랜더의 매테리얼을 받아오기
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
		{
			Material[] originalMaterials = renderer.materials;
			originalMaterialDictionary.Add(renderer, originalMaterials);
			Material[] newMaterials = new Material[renderer.materials.Length];

			for (int i = 0; i < originalMaterials.Length; i++)
			{
				Material mat = null;
				// 등록된 매테리얼인지 확인한다.
				if (cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out mat) == false)
				{
					// 새로운 매테리얼을 만들어준다.
					mat = new Material(glowmaterial);
					// By default, Unity considers a color with the property name "_Color" to be the main color;
					mat.color = originalMaterials[i].color;
					cachedGlowMaterials[mat.color] = mat;
				}
				newMaterials[i] = mat;
			}
			// 전부 등록해주기.
			glowMaterialDictionary.Add(renderer, newMaterials);
		}
	}
#endregion
}
