using System.Collections.Generic;
using Game.Scripts.Figures;
using UnityEngine;

namespace Game.Scripts.Services
{
	public class PoolFigure
	{
		public Figure Prefab { get; private set; }
		public RectTransform Container { get; private set; }

		private List<Figure> pool;

		public PoolFigure(Figure prefab, int count, RectTransform container)
		{
			this.Prefab = prefab;
			this.Container = container;
			CreatePool(count);
		}

		public bool HasFreeElement(out Figure element)
		{
			foreach (Figure obj in pool)
			{
				if(!obj.gameObject.activeInHierarchy)
				{
					element = obj;
					return true;
				}
			}
			element = null;
			return false;
		}

		public Figure GetFreeElement()
		{
			if(this.HasFreeElement(out Figure element))
			{
				return element;
			}

			return CreateObject();

		}

		public List<Figure> GetAllActiveElements()
		{
			List<Figure> elements = new List<Figure>();

			foreach (Figure obj in pool)
			{
				if(!obj.gameObject.activeInHierarchy)
				{
					elements.Add(obj);
				}
			}
			return elements;
		}

		private void CreatePool(int count)
		{
			this.pool = new List<Figure>();

			for( int i = 0; i < count; i++ )
			{
				this.CreateObject();
			}
		}

		private Figure CreateObject()
		{
			Figure createdObject = Object.Instantiate(Prefab, Container);
			createdObject.gameObject.SetActive(false);
			pool.Add(createdObject);
			createdObject.name = Prefab.name + $"_{pool.Count}";
			return createdObject;
		}
		public List<Figure> GetAllElements() => pool;

	}
}