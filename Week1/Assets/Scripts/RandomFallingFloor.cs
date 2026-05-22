using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ランダムに床を落下させるクラス
/// </summary>
public class RandomFallingPlatform : MonoBehaviour
{
    /// <summary>
    /// 床情報
    /// </summary>
    [System.Serializable]
    public class Panel
    {
        public GameObject obj;
        public Vector3 originalPosition;
    }

    [SerializeField, Tooltip("床リスト")]
    private List<Panel> panels = new List<Panel>();

    [SerializeField, Tooltip("床が落下する間隔")]
    private float fallInterval = 2f;

    [SerializeField, Tooltip("揺れる時間")]
    private float shakeDuration = 0.5f;

    [SerializeField, Tooltip("揺れ強さ")]
    private float shakeMagnitude = 0.2f;

    [SerializeField, Tooltip("リセットする床数")]
    private int resetThreshold = 8;

    private List<Panel> fallenPanels = new List<Panel>();

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        foreach (var panel in panels)
        {
            panel.originalPosition = panel.obj.transform.position;

            Rigidbody rb = panel.obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }

        InvokeRepeating(nameof(FallRandomPanel), 1f, fallInterval);
    }

    /// <summary>
    /// ランダムな床を落下させる
    /// </summary>
    private void FallRandomPanel()
    {
        if (panels.Count == 0)
        {
            CancelInvoke(nameof(FallRandomPanel));
            return;
        }

        int index = Random.Range(0, panels.Count);

        Panel panel = panels[index];

        panels.RemoveAt(index);

        StartCoroutine(ShakeAndFall(panel));

        fallenPanels.Add(panel);

        // 落下床数追加
        GameManager.Instance.AddFallCount();

        // 一定数でリセット
        if (fallenPanels.Count >= resetThreshold)
        {
            StartCoroutine(ResetPanels());
        }
    }

    /// <summary>
    /// 揺れてから落下させる
    /// </summary>
    /// <param name="panel">対象床</param>
    /// <returns>Coroutine</returns>
    private IEnumerator ShakeAndFall(Panel panel)
    {
        Vector3 originalPos = panel.obj.transform.position;

        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float z = Random.Range(-shakeMagnitude, shakeMagnitude);

            panel.obj.transform.position =
                originalPos + new Vector3(x, 0f, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        panel.obj.transform.position = originalPos;

        Rigidbody rb = panel.obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    /// <summary>
    /// 床をリセットする
    /// </summary>
    /// <returns>Coroutine</returns>
    private IEnumerator ResetPanels()
    {
        yield return new WaitForSeconds(1f);

        foreach (var panel in fallenPanels)
        {
            Rigidbody rb = panel.obj.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.linearVelocity = Vector3.zero;
            }

            panel.obj.transform.position = panel.originalPosition;

            panels.Add(panel);
        }

        fallenPanels.Clear();
    }
}