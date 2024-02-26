using System;
using Game.Scripts.Board;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Services
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioService : MonoBehaviour
	{
		[SerializeField] private AudioClip _soundRestart;
		[SerializeField] private AudioClip _soundRemoveLine;
		[SerializeField] private AudioClip _soundDropFigure;
		[SerializeField] private AudioSource _audioSource;

		public void Initialize()
		{
			BoardController.OnLinesRemove += i => PlaySound(_soundRemoveLine);
			FigureController.OnAnyFigureDroppedOnBoard += () => PlaySound(_soundDropFigure);
			UIService.OnRestartButtonClicked += () => PlaySound(_soundRestart);
			UIService.OnMuteButtonClicked += Mute;
		}

		public void PlaySound(AudioClip clip, float volume = 1f, float toneMin = .9f, float toneMax = 1.1f)
		{
			_audioSource.pitch = Random.Range(toneMin, toneMax);
			_audioSource.PlayOneShot(clip, volume);
		}

		private void Mute(bool isMute) => _audioSource.mute = isMute;

		private void OnDisable()
		{
			BoardController.OnLinesRemove -= i => PlaySound(_soundRemoveLine);
			FigureController.OnAnyFigureDroppedOnBoard -= () => PlaySound(_soundDropFigure);
			UIService.OnRestartButtonClicked -= () => PlaySound(_soundRestart);
			UIService.OnMuteButtonClicked -= Mute;
		}

	}
}