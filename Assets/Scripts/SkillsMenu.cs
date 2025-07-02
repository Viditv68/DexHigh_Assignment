using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsMenu : MonoBehaviour
{
    private const string IS_SKILL_PANEL_OPEN = "isSkillPanelOpen";
    
    [SerializeField] private Animator skillsMenuAnim;
    [SerializeField] private GameObject skillPanel;

    private bool isSkillPanelOpen = false;
    
    public List<RectTransform> menuItems;
    public float animationDuration = 0.3f;

    private bool isAnimating = false;

    public void OnMenuItemSelected(RectTransform selectedItem)
    {
        if (isAnimating) return;
        StartCoroutine(ShiftMenu(selectedItem));
    }

    private IEnumerator<WaitForSeconds> ShiftMenu(RectTransform selectedItem)
    {
        isAnimating = true;

        while (menuItems[0] != selectedItem)
        {
            List<Vector3> targetPositions = new List<Vector3>();
            foreach (var item in menuItems)
                targetPositions.Add(item.localPosition);
            
            RectTransform last = menuItems[menuItems.Count - 1];
            menuItems.RemoveAt(menuItems.Count - 1);
            menuItems.Insert(0, last);
            
            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / animationDuration);
                for (int i = 0; i < menuItems.Count; i++)
                {
                    menuItems[i].localPosition = Vector3.Lerp(menuItems[i].localPosition, targetPositions[i], t);
                }
                yield return null;
            }
            for (int i = 0; i < menuItems.Count; i++)
                menuItems[i].localPosition = targetPositions[i];
        }
        isAnimating = false;
    }


    
    public void OnSkillMenuClicked()
    {
        isSkillPanelOpen = !isSkillPanelOpen;
        skillPanel.SetActive(isSkillPanelOpen);
        skillsMenuAnim.SetBool(IS_SKILL_PANEL_OPEN, isSkillPanelOpen);
    }
}