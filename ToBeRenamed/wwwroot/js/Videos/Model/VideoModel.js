function Annotation(videoId, comment, timestamp) {
    this.videoId = videoId;
    this.comment = comment;
    this.timestamp = timestamp;
}

Annotation.prototype.submit = function(player) {
    $.ajax({
        url: apiUrls.submitAnnotation,
        data: {
            comment: this.comment,
            timestamp: this.timestamp,
            videoId: this.videoId
        },
        method: 'POST',
        dataType: 'html',
        beforeSend: function(xhr) {
            // Set header for security
            xhr.setRequestHeader("RequestVerificationToken",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function(annotationHTML) {
            prependAnnotationToAnnotationsBody(annotationHTML);
            hideCreateAnnotationControls();

            if (elements.noAnnotationsText !== null) {
                // Remove element, since there is now annotations to show
                elements.noAnnotationsText.parentElement.removeChild(noAnnotationTextElement);
            }

            // Continue playing video
            playVideo(state.player);
        }
    });
};