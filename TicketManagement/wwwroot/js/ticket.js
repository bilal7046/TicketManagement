﻿var totalPrice = 0;
async function checkIfQuantityAvailable(ticketId) {
    try {
        resetAllStatuses();
        const response = await fetch(`/api/ticket/check-availability/${ticketId}`);
        const result = await response.json();

        const ticketElement = document.getElementById(`ticket-element-${ticketId}`); // Entire ticket element
        const soldOutStatusSpan = document.getElementById(`soldout-status-${ticketId}`);
        const hurryStatusSpan = document.getElementById(`hurry-status-${ticketId}`);
        const ticketRadioButton = document.getElementById(`ticket-${ticketId}`); // Radio button
        const submitButton = document.getElementById('btnSubmit');
      

        soldOutStatusSpan.style.display = "none";
        hurryStatusSpan.style.display = "none";
       

        if (result.availableQuantity <= 0) {
            totalPrice = 0;
            soldOutStatusSpan.style.display = "inline";
            ticketRadioButton.disabled = true; // Disable radio button      
            submitButton.disabled = true; // Disable submit button      
            ticketElement.classList.add("gray-out"); // Gray out the element
        } else if (result.availableQuantity < 10) {
            hurryStatusSpan.style.display = "inline";
        }

        if (result.availableQuantity > 0) {
            totalPrice = result.price;
        }

        $('#total-price').text(`$${totalPrice}`)
    } catch (error) {
        console.error("Error checking ticket availability:", error);
    }
}
function resetAllStatuses() {
    // Reset all statuses and styles
    const ticketElements = document.querySelectorAll('.ticket-element');
    const soldOutStatuses = document.querySelectorAll('[id^="soldout-status"]');
    const hurryStatuses = document.querySelectorAll('[id^="hurry-status"]');
    const ticketRadioButtons = document.querySelectorAll('input[name="TicketTypeId"]');
    const submitButton = document.getElementById('btnSubmit');

    ticketElements.forEach(element => element.classList.remove("gray-out")); // Remove gray-out class
    soldOutStatuses.forEach(span => span.style.display = "none");
    hurryStatuses.forEach(span => span.style.display = "none");
    ticketRadioButtons.forEach(radio => radio.disabled = false); // Re-enable all radio buttons
    submitButton.disabled = false; 
}
async function applyPromoCode() {
    const promoCode = document.getElementById("promo").value.trim();

    if (!promoCode) {
        alert("Please enter a promo code.");
        return;
    }

    try {
        // Call the API to validate the promo code
        const response = await fetch(`/api/ticket/validate-promo-code?promoCode=${encodeURIComponent(promoCode)}`);

        if (!response.ok) {
            const errorText = await response.text();
            alert(errorText || "Failed to apply promo code.");
            return;
        }

        const data = await response.json();
        const discount = data.discount;

        // Update the discount display
        document.getElementById("discount-display").style.display = 'block'
        document.getElementById("discount-display").textContent = `Discount: $${discount.toFixed(2)}`;

        // Update the total price
        const totalElement = document.getElementById("total-price");

        const newTotal = totalPrice - discount;
        totalElement.textContent = newTotal.toFixed(2);

        alert("Promo code applied successfully!");
    } catch (error) {
        console.error("Error applying promo code:", error);
        alert("An error occurred. Please try again.");
    }
}