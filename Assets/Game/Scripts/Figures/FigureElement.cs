using Game.Scripts.Board;
using Game.Scripts.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Scripts.Figures
{
	public class FigureElement : MonoBehaviour
	{
		[SerializeField] private Image _image;
		[SerializeField] private LayerMask _layerMask;
		private PointerEventData _eventData;
		private Cell _emptyCellUnder;
		public bool IsActive => _image.IsActive();
		public bool AboveEmptyCell => _emptyCellUnder != null;

		public void SetupElement(bool isActive)
		{
			_image.enabled = isActive;
			_emptyCellUnder = null;
		}

		public void MarkCell(CellState cellState)
		{
			if(_emptyCellUnder != null)
			{
				_emptyCellUnder.SetCellState(cellState);
			}
		}

		public bool CastCellsUnder()
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, _layerMask);

			if(hit != default && hit.transform.TryGetComponent(out Cell cell))
			{
				if(cell != null && cell.IsEmpty && cell.IsUsable)
				{
					if(_emptyCellUnder != cell)
					{
						MarkCell(CellState.Empty);
					}
					_emptyCellUnder = cell;
					return true;
				}

			}
			else
			{
				MarkCell(CellState.Empty);
				_emptyCellUnder = null;
			}

			return false;
		}
		public bool CastLockedHolder()
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, _layerMask);

			if(hit != default && hit.transform.TryGetComponent(out LockedFigureHolder holder))
			{
				if(holder != null && holder.IsEmpty && holder.CanUseLockedHolder)
				{
					return true;
				}
			}
			return false;
		}
	}
}