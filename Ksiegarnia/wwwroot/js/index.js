// skrypt potrzebny do tworzenia losowego Isbn podczas Create/Edit w Isbns
async function generateRandomIsbn(targetInputId) {
	const response = await fetch("/Isbns/GenerateIsbn");
	if (!response.ok)
		return;
	
	const generatedIsbnValue = await response.json();
	
	const input = document.getElementById(targetInputId);
	if (!input) {
		return;
	}
	
	input.value = generatedIsbnValue; 
}