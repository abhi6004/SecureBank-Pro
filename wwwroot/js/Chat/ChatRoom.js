let activeUserId = "";
let activeSection = "";
let activeRoom = "";
let userId = "";
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.start();

// Receive messages
connection.on("ReceiveMessageHtml", function (html, section, id) {

    if (section !== activeSection) return;

    // Remove "No Chat" placeholder if present
    const firstText = $("#chat-content").text().trim();
    if (firstText === "No Chat" || firstText === "Select chat to start messaging") {
        $("#chat-content").empty();
    }

    let style = id == userId ? "text-align:right;" : "text-align:left;";
    $("#chat-content").append(`<p style="${style}">${html}</p>`);
});



// Open a chat
window.OpenChat = function (id, section, roleAllowed = "" , _userId) {
    // Restrict room chat by role
    if (section === "room" && roleAllowed && roleAllowed !== userRole) {
        alert("You don't have permission to access this room.");
        return;
    }

    activeUserId = id;
    activeSection = section;
    activeRoom = id;
    userId = _userId;

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
window.SendChat = function (message, receiver, section, room) {
    connection.invoke("SendChat", message, receiver, section, room);
};
