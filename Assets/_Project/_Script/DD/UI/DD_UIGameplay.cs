using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DD_UIGameplay : MonoBehaviour {
    [SerializeField] private List<Image> lsArrowTop = new List<Image>();

    [SerializeField] private Transform transTarget;
    public List<Transform> lsTransSpawnArrowBot = new List<Transform>();

    public float GetDistanceMoveArrow() {
        return Vector2.Distance(transTarget.position, lsTransSpawnArrowBot[0].position);
    }

    public List<Transform> GetListTransformSpawnArrow() {
        return lsTransSpawnArrowBot;
    }

    public List<Transform> GetListTargetArrow() {
        List<Transform> lsTransTarget = new List<Transform>();
        for (int i = 0; i < lsArrowTop.Count; i++) {
            lsTransTarget.Add(lsArrowTop[i].transform);
        }

        return lsTransTarget;
    }
}