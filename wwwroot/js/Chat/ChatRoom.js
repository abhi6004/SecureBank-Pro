let activeUserId = "";
let activeSection = "";
let activeRoom = "";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.start();

// Receive messages
connection.on("ReceiveMessage", function (chat) {
    if (chat.section !== activeSection) return;

    let content = $("#chat-content");
    let msgText = chat.section === "general"
        ? `${chat.SenderName} (${chat.Department}): ${chat.messageText}`
        : `${chat.SenderName}: ${chat.messageText}`;

    content.append(`<p style="text-align:${chat.SenderId === activeUserId ? 'right' : 'left'}">${msgText}</p>`);
});

// Open a chat
window.OpenChat = function (id, section, roleAllowed = "") {
    // Restrict room chat by role
    if (section === "room" && roleAllowed && roleAllowed !== userRole) {
        alert("You don't have permission to access this room.");
        return;
    }

    activeUserId = id;
    activeSection = section;
    activeRoom = id;

    $("#chat-title").text(section === "general" ? "General Room" : id);

    GetUserChat(id, section);
};

// Get chat messages
window.GetUserChat = function (userId, section) {
    $.get("/Chat/UserChat", { Reciver: userId, Section: section }, function (html) {
        $("#chat-content").html(html);
    });
};

// Send chat from input box
window.SendActiveChat = function () {
    let box = $("#main-message-box");
    let message = box.val().trim();
    if (!message) return;

    SendChat(message, activeUserId, activeSection, activeRoom);
    box.val("");
};

// Send chat to server
window.SendChat = function (message, id, section, room) {
    connection.invoke("SendChat", message, id, section, room);
};
