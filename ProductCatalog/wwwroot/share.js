window.shareProduct = async function (title, price) {
    const city = "Chicago";

    const message =
        `${title} - ${price} from ${city} added to list`;

    if (navigator.share) {
        await navigator.share({
            title: "Product Added",
            text: message
        });
    }
    else {
        await navigator.clipboard.writeText(message);
        alert("Copied to clipboard");
    }
}