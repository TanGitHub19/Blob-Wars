using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public GameObject[] pages; // Array of tutorial pages
    private int currentPage = 0; // Track current page index

    public Button nextButton;
    public Button prevButton;

    void Start()
    {
        // Assign the button functions in code
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);

        UpdatePages(); // Show the correct page on start
    }

    void UpdatePages()
    {
        // Show only the current page
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        // Enable or disable buttons based on the current page
        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePages();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePages();
        }
    }
}
