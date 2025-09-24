$(function () {
    // Build and start SignalR connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.start()
        .then(() => console.log("SignalR connected"))
        .catch(err => console.error("Connection failed:", err));

    // Register ReceiveMessage handler ONCE
    connection.on("ReceiveMessage", (chat) => {
        console.log("Message received:", chat);

        const appendMessage = (containerId, messageText, align) => {
            const container = document.getElementById(containerId);
            if (!container) return;
            const p = document.createElement("p");
            p.innerText = messageText;
            p.style.textAlign = align;
            container.appendChild(p);
            container.scrollTop = container.scrollHeight; // auto-scroll
        };

        if (chat.section === 'private') {
            const [id, fullName] = chat.room.split("-");
            appendMessage(id, chat.messageText, "right");
            appendMessage(fullName, chat.messageText, "left");
        }
        else if (chat.section === 'room') {
            appendMessage(chat.room, chat.messageText, "right");
        }
        else if (chat.section === 'general') {
            appendMessage("general", chat.messageText, "right");
        }
    });

    // Navigate to all users
    window.GetAllUsers = function (section) {
        window.location.href = "/Chat/ChatRoom?section=" + encodeURIComponent(section);
    }

    // Load chat history for a user or room
    window.GetUserChat = function (userId, section) {
        fetch(`/Chat/UserChat?Reciver=${encodeURIComponent(userId)}&Section=${encodeURIComponent(section)}`)
            .then(response => response.text())
            .then(html => {
                let containerId = (section === 'general') ? 'general' : userId;
                const container = document.getElementById(containerId);
                if (container) container.innerHTML = html;
            })
            .catch(err => console.error(err));
    }

    // Send chat message
    window.SendChat = function (message, id, section, room) {
        if (!message.trim()) return;
        connection.invoke("SendChat", message, id, section, room)
            .catch(err => console.error("Invoke failed:", err));
    }
});
