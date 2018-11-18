﻿/**
 * 
 */
function loadIFramePlayerAPI() {
    
}

/**
 * Gets the video url
 * @returns {string} - The video url (identifier) used by the youtube API to get the video
 */
function getVideoUrl() {
    return elements.videoUrl.value;
}

/**
 * Gets the video id
 * @returns {string} - The id of the video used in the database
 */
function getVideoId() {
    return elements.videoId.value;
}

function getCreatedAnnotationComment() {
    return elements.createAnnotationTextarea.value;
}

function getCurrentYoutubeTime(player) {
    return player.getCurrentTime();
}

/**
 * Hides the create annotation controls
 */
function hideCreateAnnotationControls() {
    elements.createAnnotation.classList.add('hidden');
    elements.createAnnotationTextarea.value = '';
}

/**
 * Pauses the video
 * @param player - The youtube player
 */
function pauseVideo(player) {
    player.pauseVideo();
}

/**
 * Plays the video
 * @param player - the youtube player
 */
function playVideo(player) {
    player.playVideo();
}

/**
 * Sets up the annotation controls for the user so that they can write a new annotation. This should only be called
 * when the controls are already hidden, and they need to be displayed to the user.
 */
function setupAnnotationControls() {
    // Set up controls
    elements.newAnnotationTimestamp.innerText = getTimestampToDisplay(state.player.getCurrentTime());
    // Create annotation controls are hidden, so display them
    elements.createAnnotation.classList.remove('hidden');
}

/**
 * Checks if the create annotation controls are hidden
 * @returns {boolean} - true if create annotation controls are hidden, false otherwise
 */
function areCreateAnnotationControlsHidden() {
    return elements.createAnnotation.classList.contains('hidden');
}

/**
 * 
 * @param target
 * @returns {boolean}
 */
function isClickedButtonSubmitAnnotationButton(target) {
    return target.classList.contains(classNames.submitAnnotation);
}

function isClickedButtonShowRepliesButton(target) {
    return target.classList.contains(classNames.toggleRepliesButton);
}

/**
 * Checks if the replies for the annotation (target) are currently hidden by looking for the hidden class
 * @param target - The annotation HTML element
 * @returns {boolean} - True if hidden, false otherwise
 */
function areRepliesHidden(target) {
    return $(target.closest('.annotation-wrapper').lastElementChild).hasClass('hidden');;
}

function displayReplies(annotationElement) {
    $(annotationElement.lastElementChild).removeClass('hidden');
    changeToggleRepliesTextToHide(annotationElement);
}

function hideReplies(annotationElement) {
    $(annotationElement.lastElementChild).addClass('hidden');
    changeToggleRepliesTextToShow(annotationElement);
}

function changeToggleRepliesTextToShow(annotationElement) {
    annotationElement.querySelector('.' + classNames.toggleRepliesButton).innerHTML = getToggleRepliesShowHTML();
}

function getToggleRepliesShowHTML() {
    return 'Show Replies<span class="glyphicon glyphicon-menu-down"></span>';
}

function changeToggleRepliesTextToHide(annotationElement) {
    annotationElement.querySelector('.' + classNames.toggleRepliesButton).innerHTML = getToggleRepliesHideHTML();
}

function getToggleRepliesHideHTML() {
    return 'Hide Replies<span class="glyphicon glyphicon-menu-up"></span>';
}

/**
 * Prepends the annotation HTML to the annotations body
 * @param annotationHTML - HTML created by the backend that represents a single annotation
 */
function prependAnnotationToAnnotationsBody(annotationHTML){
    $(elements.annotationsBody).prepend(annotationHTML);
}

function renderReplyControls(annotationElement) {
    var html = getCreateReplyControlsHTML();
    
    annotationElement.querySelector('.panel').insertAdjacentHTML('afterend', html);
}

function getCreateReplyControlsHTML() {

    return `<div class="create-reply-container">
                <div class="panel panel-default">
                    <div class="panel panel-heading reply-header">
                        <span class="annotation-options glyphicon glyphicon-option-horizontal" aria-hidden="true"></span>
                        <!--<span class="annotation-author-time block">-->
                        <!--</span>-->
                    </div>
                    <div class="panel-body reply-body">
                        <div class="reply-text-wrapper row">
                            <textarea></textarea>
                            <button type="button" class="submit-reply btn btn-success btn-sm">Submit</button>
                            <button type="button" class="cancel-reply btn btn-secondary btn-sm">Cancel</button>
                        </div>
                    </div>
                </div>
                <hr>
            </div>`;
}

function isClickedButtonCreateReplyButton(target) {
    return target.classList.contains('reply-button');
}

function isClickedButtonCancelCreateReplyButton(target) {
    return target.classList.contains(classNames.cancelCreateReplyButton);
}

function areCreateReplyControlsDisplayed(target) {
    return target.closest('.' + classNames.annotationWrapper).getElementsByClassName(classNames.createReplyControls).length > 0;
}

function doesAnnotationHaveReplies(annotationElement) {
    return annotationElement.getElementsByClassName(classNames.toggleRepliesButton).length > 0;
}

function isClickedButtonSubmitReplyButton(target) {
    return target.classList.contains(classNames.submitReply);
}

function prependReplyToRepliesBody(annotationElement, replyHTML){
    var repliesBody = annotationElement.querySelector('.' + classNames.annotationReplies);
    $(repliesBody).prepend(replyHTML);
}

function removeCreateReplyControls(annotationElement) {
    var createReplyControls = annotationElement.querySelector('.' + classNames.createReplyControls);
    annotationElement.removeChild(createReplyControls);
}

function getCreatedReplyText(annotationElement) {
    return annotationElement.querySelector('textarea').value;
}

function doesAnnotationElementHaveToggleRepliesButton(annotationElement) {
    return annotationElement.getElementsByClassName(classNames.toggleRepliesButton).length > 0;
}

function renderToggleRepliesButton(annotationElement) {
    annotationElement.querySelector('.panel-body').insertAdjacentHTML('beforeend', getToggleRepliesDefaultHTML());
}

function getToggleRepliesDefaultHTML() {
    return `
        <div class="toggle-replies-wrapper">
            <a class="annotation-text toggle-replies" href="#">${getToggleRepliesShowHTML()}</a>
        </div>
    `
}

/**
 * Using all of the annotation elements on the page, get the user id's and display names of
 * the users who posted those annotations and return them
 * @param annotationElements - An array of the annotation elements on the page
 * @return {object} - An object of user id's mapped to user display names. Ex: {1: 'Kyle Jones'}
 */
function getUserIdsAndNames(annotationElements) {
    var userIdsAndNames = {};
    
    for(var i = 0; i < annotationElements.length; i++) {
        var annotationElement = annotationElements.item(i);

        addUserIdAndNameFromElement(annotationElement, userIdsAndNames);
    }
    
    return userIdsAndNames;
}

/**
 * Add the user id and display name of an individual annotation element to the userIdsAndNames param
 * @param annotationElement - The annotation where the user id and display name will come from
 * @param userIdsAndNames - A reference to an object where user ids are mapped to display names
 */
function addUserIdAndNameFromElement(annotationElement, userIdsAndNames) {
    var userId = annotationElement.dataset['authorId'];

    if(userIdsAndNames[userId] === undefined) {
        userIdsAndNames[userId] = annotationElement.querySelector(selectors.displayName).innerText;
        insertIntoFilterByUserDropdown(userId, userIdsAndNames[userId]);
    }
}

/**
 * Gets the element where the annotations are stored
 * @returns {HTMLElement}
 */
function getAnnotationElements() {
    return document.getElementById(idNames.annotationsBody);
}

/**
 * Gets the elements where the replies are stored
 * @returns {HTMLCollectionOf<Element>}
 */
function getReplyElements() {
    return document.getElementsByClassName('reply-container');
}

function insertIntoFilterByUserDropdown(userId, displayName) {
    var dropdown = elements.annotations.querySelector(selectors.filterAnnotationsList);
    
    var listElement = document.createElement('li');
    var listButton = document.createElement('a');
    var text = document.createTextNode(displayName);
    listButton.appendChild(text);
    listElement.appendChild(listButton);
    
    listButton.dataset['authorId'] = userId;
    listButton.href = '#';
    
    dropdown.appendChild(listElement);
}

/**
 * Renders the annotation options dropdown html for all annotations on the page
 */
function renderAnnotationOptionsDropdowns() {
    for(var i = 0; i< state.annotationElements.children.length; i++) {
        var annotation = state.annotationElements.children.item(i);
        renderAnnotationOptionsDropdown(annotation);
    }
}

/**
 * Renders the reply options dropdown html for all annotations on the page
 */
function renderReplyOptionsDropdowns() {
    for(var i = 0; i< state.replyElements.length; i++) {
        var reply = state.replyElements.item(i);
        
        renderReplyOptionsDropdown(reply);
    }
}

/**
 * Renders the reply options dropdown html for a single reply element
 * @param reply -  the reply element
 */
function renderReplyOptionsDropdown(reply) {
    var replyOptionsUl = reply.querySelector('.reply-options-ul');

    if(reply.dataset['authorId'] === state.currentUserId) {
        var editListElement = document.createElement('li');
        var editListButton = document.createElement('a');
        var editText = document.createTextNode('Edit');
        editListButton.appendChild(editText);
        editListElement.appendChild(editListButton);
        editListButton.href = '#';
        editListButton.classList.add(classNames.editReply);

        replyOptionsUl.appendChild(editListElement);

        var deleteListElement = document.createElement('li');
        var deleteListButton = document.createElement('a');
        var deleteText = document.createTextNode('Delete');
        deleteListButton.appendChild(deleteText);
        deleteListElement.appendChild(deleteListButton);
        deleteListButton.href = '#';
        deleteListButton.classList.add(classNames.deleteReply);

        replyOptionsUl.appendChild(deleteListElement);
    } else {
        // reply is not owned by current user
        var listElement = document.createElement('li');
        var text = document.createTextNode('No Options');
        listElement.appendChild(text);

        replyOptionsUl.appendChild(listElement);
    }
}

/**
 * Unhides the message that tells the user that there are no annotations
 */
function unhideNoAnnotationText() {
    elements.annotations.querySelector(selectors.noAnnotationsText).classList.remove('hidden');
}

/**
 * Hides the message that tells the user that there are no annotations
 */
function hideNoAnnotationText() {
    elements.annotations.querySelector(selectors.noAnnotationsText).classList.add('hidden');
}

/**
 * Renders the annotation options dropdown html for a single annotation element
 * @param annotation -  the annotation element
 */
function renderAnnotationOptionsDropdown(annotation) {
    var annotationOptionsUl = annotation.querySelector('.annotation-options-ul');

    if(annotation.dataset['authorId'] === state.currentUserId) {
        var editListElement = document.createElement('li');
        var editListButton = document.createElement('a');
        var editText = document.createTextNode('Edit');
        editListButton.appendChild(editText);
        editListElement.appendChild(editListButton);
        editListButton.href = '#';
        editListButton.classList.add(classNames.editAnnotation);

        annotationOptionsUl.appendChild(editListElement);

        var deleteListElement = document.createElement('li');
        var deleteListButton = document.createElement('a');
        var deleteText = document.createTextNode('Delete');
        deleteListButton.appendChild(deleteText);
        deleteListElement.appendChild(deleteListButton);
        deleteListButton.href = '#';
        deleteListButton.classList.add(classNames.deleteAnnotation);

        annotationOptionsUl.appendChild(deleteListElement);
    } else {
        // annotation is not owned by current user
        var listElement = document.createElement('li');
        var text = document.createTextNode('No Options');
        listElement.appendChild(text);

        annotationOptionsUl.appendChild(listElement);
    }
}

/**
 * Gets the HTML for the edit annotation controls
 * @returns {string}
 */
function getEditAnnotationControlsHTML() {
    return `
        <div class="edit-annotation-text-wrapper row">
            <textarea></textarea>
            <button type="button" class="submit-edit-annotation btn btn-success btn-sm">Submit</button>
            <button type="button" class="cancel-edit-annotation btn btn-secondary btn-sm">Cancel</button>
        </div>
    `;
}

/**
 * Gets the HTML for the edit annotation controls
 * @returns {string}
 */
function getEditReplyControlsHTML() {
    return `
        <div class="edit-annotation-text-wrapper row">
            <textarea></textarea>
            <button type="button" class="submit-edit-reply btn btn-success btn-sm">Submit</button>
            <button type="button" class="cancel-edit-reply btn btn-secondary btn-sm">Cancel</button>
        </div>
    `;
}

/**
 * Hides the annotation text.
 * This is useful when you need to edit the text, and hide it before displaying the edit controls
 * @param annotationElementBody - The annotation element's body element
 */
function hideAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(selectors.annotationText).classList.add('hidden');
}

/**
 * Hides the reply text.
 * This is useful when you need to edit the text, and hide it before displaying the edit controls
 * @param replyElementBody - The reply element's body element
 */
function hideReplyText(replyElementBody) {
    replyElementBody.querySelector(selectors.replyText).classList.add('hidden');
}

/**
 * Removes the annotation element from the view.
 * Useful when an annotation gets deleted.
 * @param annotationElement - The annotation to be deleted
 */
function removeAnnotation(annotationElement) {
    annotationElement.parentElement.removeChild(annotationElement);
}

/**
 * Unhides the annotation text.
 * Useful when closing the edit annotation controls and showing the existing annotation text again
 * @param annotationElementBody
 */
function unhideAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(selectors.annotationText).classList.remove('hidden');
}

/**
 * Updates the annotation text.
 * Useful after the annotation gets updated.
 * @param annotationElementBody - The annotation element's body element
 */
function updateAnnotationText(annotationElementBody) {
    annotationElementBody.querySelector(selectors.annotationText).innerText = document.querySelector(selectors.editAnnotationText).value;
}

/**
 * Renders the edit annotation controls
 * @param annotationElementBody - The annotation element's body element, where the controls will be displayed
 */
function renderEditAnnotationControls(annotationElementBody) {
    var html = getEditAnnotationControlsHTML();
    
    $(annotationElementBody).prepend(html);
    
    // Add existing annotation text to textarea
    var annotationText = annotationElementBody.querySelector(selectors.annotationText).innerText.trim();
    annotationElementBody.querySelector('textarea').value = annotationText;
}

/**
 * Renders the edit reply controls
 * @param replyElementBody - The reply element's body element, where the controls will be displayed
 */
function renderEditReplyControls(replyElementBody) {
    var html = getEditReplyControlsHTML();

    $(replyElementBody).prepend(html);

    // Add existing annotation text to textarea
    var replyText = replyElementBody.querySelector(selectors.replyText).innerText.trim();
    replyElementBody.querySelector('textarea').value = replyText;
}

/**
 * Removes the edit annotation controls from the view
 * @param annotationElementBody - The annotation element's body element
 */
function removeEditAnnotationControls(annotationElementBody) {
    var editAnnotationTextWrapper = annotationElementBody.querySelector(selectors.editAnnotationTextWrapper);
    annotationElementBody.removeChild(editAnnotationTextWrapper);
}

function setCurrentUserId() {
    state.currentUserId = document.querySelector('#user-id').value;
}

function doesVideoHaveAnnotations() {
    return elements.annotations.querySelector(selectors.noAnnotationsText) === null;
}

/**
 * Highlight or unhighlight (if already highlighted) the clickedEntryElement
 * @param clickedEntryElement - the filter dropdown entry that was clicked
 */
function updateHighlightedUser(clickedEntryElement) {
    // add 'active' class to classlist if it already isn't in it
    if(clickedEntryElement.classList.contains('active')) {
        clickedEntryElement.classList.remove('active');
    } else {
        clickedEntryElement.classList.add('active');
    }
}

/**
 * Update the state object that holds the data about which user id's are currently
 * being filtered
 * @param clickedEntryElement - The entry in the filter dropdown that was clicked
 */
function updateFilterUserIdState(clickedEntryElement) {
    var userId = clickedEntryElement.dataset['authorId'];
    
    if(state.filterUserId.has(userId)) {
        // user is already being filtered, turn filtering off for the user
        state.filterUserId.delete(userId);
    } else {
        // user is not being filtered, so turn it on for the user
        state.filterUserId.add(userId);
    }
}

/**
 * Filter the annotations so that the annotations that belong to any user
 * id's in the state object that holds the filter data are displayed, and any
 * user id's that aren't in it are hidden.
 */
function filterAnnotationsByUserId() {
    var annotationElements = state.annotationElements.children;
    
    for(var i = 0; i< annotationElements.length; i++) {
        var annotation = annotationElements.item(i);
        filterAnnotationByUserId(annotation);
    }
}

/**
 * Hides or displays an annotation according to state's filter user data
 * @param annotation - The annotation element that will be displayed or hidden
 */
function filterAnnotationByUserId(annotation) {
    var annotationUserId = annotation.dataset['authorId'];

    if(state.filterUserId.size === 0) {
        // No annotations are being filtered, display all
        annotation.classList.remove('hidden');
    } else if(state.filterUserId.has(annotationUserId)) {
        // Filter by user id, so make sure it's being displayed
        annotation.classList.remove('hidden');
    } else {
        // User id is not in filter, so hide it
        annotation.classList.add('hidden');
    }
}
