using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD_Core;
using UnityEngine;
using Newtonsoft.Json;

public class DD_GameManager : MonoBehaviour {
    [SerializeField] public DD_UIGameplay uiGameplay;

    [Header("------------Transform & Game Object------------")] [SerializeField]
    private List<GameObject> lsPrefabArrows = new List<GameObject>();

    [SerializeField] private List<Transform> lsContainSpawnArrow = new List<Transform>();

    [SerializeField] private List<Transform> lsContainSpawnEnemyArrow = new List<Transform>();

    [SerializeField] private List<Transform> lsTargetArrow = new List<Transform>();

    [SerializeField] private List<Transform> lsPositionSpawnArrow = new List<Transform>();

    [Header("------------Data------------")]
    private List<DD_ArrowDataItem> lsArrowDataItems = new List<DD_ArrowDataItem>();

    [Header("------------Variable------------")]
    private float prevTimeArrow = 0;

    private float distanceMoveArrow = 0;
    private int curIndexArrow = 0;

    private float timeMoveArrow;
    private float defaultTimeMoveArrow = 1.8f;


    public void SetupGameplayUI(float lengthSong) {
        prevTimeArrow = 0;
        distanceMoveArrow = uiGameplay.GetDistanceMoveArrow();
        timeMoveArrow = defaultTimeMoveArrow * 1;

        lsPositionSpawnArrow = uiGameplay.GetListTransformSpawnArrow();
        lsTargetArrow = uiGameplay.GetListTargetArrow();

        DD_RootItem rootItem =
            JsonConvert.DeserializeObject<DD_RootItem>(Resources.Load<TextAsset>("Jsons/tutorial-easy").text);
        DD_SongItem songItem = rootItem.songItem;

        lsArrowDataItems.Clear();
        for (int i = 0; i < songItem.notes.Count; i++) {
            for (int j = 0; j < songItem.notes[i].sectionNotes.Count; j++) {
                DD_ArrowDataItem arrowDataItem = new DD_ArrowDataItem(songItem.notes[i].sectionNotes[j][0],
                    (int)songItem.notes[i].sectionNotes[j][1], songItem.notes[i].sectionNotes[j][2],
                    songItem.notes[i].mustHitSection);
                lsArrowDataItems.Add(arrowDataItem);
            }
        }

        lsArrowDataItems.Sort(SortByTimeAppear);
    }

    public void LoadNoteNew(float time) {
        if (curIndexArrow == lsArrowDataItems.Count - 1) {
            if (lsArrowDataItems[curIndexArrow - 1].timeAppear > time * 1000) {
                return;
            }
            else {
                if ((lsArrowDataItems[curIndexArrow].timeAppear / 1000 - time) < -0.001f &&
                    (lsArrowDataItems[curIndexArrow].timeAppear / 1000 - time) >= -0.15f) {
                    return;
                }
            }
        }
        else {
            if (lsArrowDataItems[curIndexArrow].timeAppear == 0 || lsArrowDataItems[curIndexArrow].timeAppear < 1000) {
                return;
            }
            else {
                if (lsArrowDataItems[curIndexArrow].timeAppear > time * 1000) {
                    return;
                }
                else {
                    if (lsArrowDataItems[curIndexArrow + 1].timeAppear > time * 1000) {
                    }
                }
            }
        }
    }

    private int SortByTimeAppear(DD_ArrowDataItem obj1, DD_ArrowDataItem obj2) {
        return obj1.timeAppear.CompareTo(obj1.timeAppear);
    }

    private void CreateArrow() {
        if (lsArrowDataItems[curIndexArrow] != null) {
            int indexArrowClone = lsArrowDataItems[curIndexArrow].indexArrow;
            int sumArrow = lsArrowDataItems.Count;
            if (lsArrowDataItems[curIndexArrow].mustHit) {
                GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone], lsContainSpawnArrow[indexArrowClone]);
                goArrow.transform.localPosition = lsPositionSpawnArrow[indexArrowClone].position;
                DD_Arrow arrowMove = goArrow.GetComponent<DD_Arrow>();
                arrowMove.SetupArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                    lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                    distanceMoveArrow, curIndexArrow, sumArrow);
            }
            else {
                GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone], lsContainSpawnEnemyArrow[indexArrowClone]);
                goArrow.transform.localPosition = lsPositionSpawnArrow[indexArrowClone].position;
                DD_Arrow arrowMove = goArrow.GetComponent<DD_Arrow>();
                arrowMove.SetupArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                    lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                    distanceMoveArrow, curIndexArrow, sumArrow);

            }
        }
        
    }

    private void GetSongGameplay(int indexMod, string nameSong) {
        if (indexMod == 0) {
            AudioClip songAudioClip = Resources.Load("Sounds/Inst-" + nameSong) as AudioClip;
            DD_SoundManager.Instance.AddSoundBGM(songAudioClip);
        }
        else {
        }
    }
}

[Serializable]
public class DD_ArrowDataItem {
    public float timeAppear;
    public int indexArrow;
    public float timeTail;
    public bool mustHit;

    public DD_ArrowDataItem(float timeAppear, int indexArrow, float timeTail, bool mustHit) {
        this.timeAppear = timeAppear;
        this.indexArrow = indexArrow;
        this.timeTail = timeTail;
        this.mustHit = mustHit;
    }
}

[Serializable]
public class DD_NoteSongItem {
    public int lengthInStep;
    public bool mustHitSection;
    public List<float[]> sectionNotes = new List<float[]>();

    public DD_NoteSongItem(List<float[]> sectionNotes, int lengthInStep, bool mustHitSection) {
        this.sectionNotes = sectionNotes;
        this.lengthInStep = lengthInStep;
        this.mustHitSection = mustHitSection;
    }
}

[Serializable]
public class DD_SongItem {
    public List<DD_NoteSongItem> notes = new List<DD_NoteSongItem>();
}

[Serializable]
public class DD_RootItem {
    public DD_SongItem songItem;
}